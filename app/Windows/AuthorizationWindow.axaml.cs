using System;
using System.Linq;
using System.Threading.Tasks;
using app.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore.Internal;

namespace app.Windows;

public partial class AuthorizationWindow : Window
{
    private readonly int _attempts = 3;
    
    public AuthorizationWindow()
    {
        InitializeComponent();
    }

    private async Task OpenMessageWindow(string result) // error, success, blocked
    {
        var messageWindow = new MessageWindow();
        
        switch (result)
        {
            case "error":
                messageWindow.Message.Text = 
                    "Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные";
                await messageWindow.ShowDialog<bool>(this);
                break;
            case "success":
                messageWindow.Message.Text = 
                    "Вы успешно авторизовались";
                await messageWindow.ShowDialog<bool>(this);
                break;
            case "blocked":
                messageWindow.Message.Text = 
                    "Вы заблокированы. Обратитесь к администратору";
                await messageWindow.ShowDialog<bool>(this);
                break;
        }
    }

    private static async Task<User?> GetUser(string login, string password)
    {
        await using var context = new MkarpovDe092025Context();
        var user = context.Users.FirstOrDefault(
            u => u.Login == login || u.Password == password);
            
        return user;
    }

    private async Task UpdateUser(User user)
    {
        await using var context = new MkarpovDe092025Context();
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    private async void AuthButtonOnClick(object? sender, RoutedEventArgs e)
    {
        User upUser;
        var log = TBLogin.Text;
        var pass = TBPassword.Text;
        var userWindow = new UserWindow();

        if (string.IsNullOrWhiteSpace(log) || string.IsNullOrWhiteSpace(pass))
        {
            await OpenMessageWindow("error");
            return;
        }

        // 1 context, так нельзя нужно поменять
        var user = await GetUser(log, pass);

        if (user == null)
        {
            await OpenMessageWindow("error");
            return;
        }
                    
        if (user.IsBlocked)
        {
            await OpenMessageWindow("blocked");
            return;
        }
            
        if (user.Password != pass || user.Login != log)
        {
            if (user.LoginAttempts < _attempts || user.LoginAttempts is null)
            {
                var loginAttempts = user.LoginAttempts ?? 0; 
                
                upUser = new User()
                {
                    UserId = user.UserId,
                    IsBlocked = user.IsBlocked,
                    Password = user.Password,
                    Login = user.Login,
                    LastLoginDate = user.LastLoginDate,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = user.RoleId,
                    LoginAttempts = loginAttempts + 1
                };
                        
                await UpdateUser(upUser);
            }
            
            if (!user.IsBlocked & user.LoginAttempts == _attempts 
                || user.LastLoginDate < DateTime.Now - TimeSpan.FromDays(30))
            {
                upUser = new User()
                {
                    UserId = user.UserId,
                    IsBlocked = true,
                    Password = user.Password,
                    Login = user.Login,
                    LastLoginDate = user.LastLoginDate,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = user.RoleId,
                    LoginAttempts = user.LoginAttempts
                };
            
                await UpdateUser(upUser);
                
                await OpenMessageWindow("blocked");
                return;
            }
                        
            await OpenMessageWindow("error");
            return;
        }
                    
        await OpenMessageWindow("success");
            
        upUser = new User()
        {
            UserId = user.UserId,
            IsBlocked = false,
            Password = user.Password,
            Login = user.Login,
            LastLoginDate = DateTime.Today,
            FirstName = user.FirstName,
            LastName = user.LastName,
            RoleId = user.RoleId,
            LoginAttempts = 0
        };
                    
        await UpdateUser(upUser);

        await userWindow.ShowDialog<bool>(this);
    }

}