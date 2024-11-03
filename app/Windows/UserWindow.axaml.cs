using System;
using System.Threading.Tasks;
using app.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using Mysqlx.Cursor;

namespace app.Windows;

public partial class UserWindow : Window
{
    private readonly User _user;
    public UserWindow(User user)
    {
        _user = user;
        InitializeComponent();
        /*ContentTemplate = (DataTemplate)Resources["CustomerTemplate"]!;*/
        
        /*ContentTemplate = (DataTemplate)Resources["AdministratorTemplate"]!;*/
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
}