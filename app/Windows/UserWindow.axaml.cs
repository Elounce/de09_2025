using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using app.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Cursor;

namespace app.Windows;

public partial class UserWindow : Window, INotifyPropertyChanged
{
    private readonly User _user;
    public List<User>? UsersList {get; private set; }
    public IEnumerable<String> RolesList { get; private set; }
    
    public new event PropertyChangedEventHandler? PropertyChanged;

    public UserWindow(User user)
    {
        _user = user;
        InitializeComponent();
        DataContext = this;
        
        if (_user.RoleId.Equals(2))
        {
            UserView.IsEnabled = true;
            UserView.IsVisible = true;
        }
        else
        {
            AdminView.IsEnabled = true;
            AdminView.IsVisible = true;
            _ = GetUsers();
            _ = GetRoles();
        }
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task UpdateUser(User user)
    {
        try
        {
            await using (var context = new MkarpovDe092025Context())
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            await OpenMessageWindow($"{e.Message}");
        }
        
    }

    private async Task GetRoles()
    {
        await using (var context = new MkarpovDe092025Context())
        {
            var roles = context.Roles
                .Select(r => r.Name)
                .ToList();
            RolesList = roles.Prepend("");
        }
        
        NotifyPropertyChanged(nameof(RolesList));
    }

    private async Task GetUsers()
    {
        await using (var context = new MkarpovDe092025Context())
        {
            UsersList = context.Users
                .Where(u => u.RoleId.Equals(2))
                .Include(u => u.Role)
                .ToList();
        }
        NotifyPropertyChanged(nameof(UsersList));
    }

    private async Task OpenMessageWindow(string message)
    {
        var msgWindow = new MessageWindow();
        msgWindow.Message.Text = message;
        await msgWindow.ShowDialog<bool>(this);
    }

    private async void ChangePassword_OnClick(object? sender, RoutedEventArgs e)
    {
        var curPass = CurrentPassword.Text;
        var newPass = NewPassword.Text;
        var newPassConf = NewPasswordConfirm.Text;

        if (curPass == "" || newPass == "" || newPassConf == "")
        {
            await OpenMessageWindow("Все поля должны быть заполнены!");
            return;
        }

        if (curPass != _user.Password)
        {
            await OpenMessageWindow("Неверный пароль. Попробуйте ещё раз.");
            return;
        }

        if (newPass != newPassConf)
        {
            await OpenMessageWindow("Новый пароль не совпадает.");
            return;
        }

        var upUser = new User()
        {
            UserId = _user.UserId,
            IsBlocked = _user.IsBlocked,
            Password = newPassConf,
            FirstName = _user.FirstName,
            LastName = _user.LastName,
            RoleId = _user.RoleId,
            LoginAttempts = _user.LoginAttempts,
            LastLoginDate = _user.LastLoginDate,
            Login = _user.Login,
            Role = _user.Role
        };
        
        await UpdateUser(upUser);
        await OpenMessageWindow("Пароль успешно изменён.");
        Close(true);

    }

    private async void NewUser_OnClick(object? sender, RoutedEventArgs e)
    {
        var newUserWindow = new NewUserWindow(RolesList);
        await newUserWindow.ShowDialog(this);
    }

    // Изменение данных юзеров работает некорректно
    private async void ChangeUser_OnClick(object? sender, RoutedEventArgs e)
    {
        var user = AdminListBox.SelectedItem as User;
        
        if (user == null)
            return;
        
        if (string.IsNullOrWhiteSpace(user.Login) 
            || string.IsNullOrWhiteSpace(user.Password)
            || user.RoleId == 0)
            return;
        
        await UpdateUser(user);
        await OpenMessageWindow("Данные пользователя изменены");
        await GetUsers();
    }
}