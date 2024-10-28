using System;
using System.Linq;
using System.Threading.Tasks;
using app.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace app.Windows;

public partial class AuthorizationWindow : Window
{
    private MkarpovDe092025Context _context;
    private UserAuth _auth;

    private int attempts = 3;
    
    public AuthorizationWindow()
    {
        _auth = new UserAuth();
        _context = new MkarpovDe092025Context();
        InitializeComponent();
    }

    private async void AuthButtonOnClick(object? sender, RoutedEventArgs e)
    {
        var log = TBLogin.Text;
        var pass = TBPassword.Text;
        var messageWindow = new MessageWindow();
        var userWindow = new UserWindow();

        if (string.IsNullOrWhiteSpace(log) || string.IsNullOrWhiteSpace(pass))
        {
            messageWindow.Message.Text = 
                "Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные";
            await messageWindow.ShowDialog<bool>(this);
            return;
        }
        
        var user = _context.Users.FirstOrDefault(
            u => u.Login == log || u.Password == pass);

        if (user == null)
        {
            messageWindow.Message.Text = 
                "Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные";
            await messageWindow.ShowDialog<bool>(this);
            return;
        }

        if (user.Password == pass && user.Login == log)
        {
            messageWindow.Message.Text = 
                "Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные";
            await messageWindow.ShowDialog<bool>(this);
            return;
        }
        
        if (user.LoginAttempts == attempts)
        {
                                                                   
        }
        
        
        
        messageWindow.Message.Text = "Вы успешно авторизовались";
        await messageWindow.ShowDialog<bool>(this);

        await userWindow.ShowDialog<bool>(this);
    }

}