using System;
using System.Linq;
using app.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace app.Windows;

public partial class AuthorizationWindow : Window
{
    private MkarpovDe092025Context _context;
    private UserAuth _auth;
    
    public MessageWindow messageWindow;
    public UserWindow userWindow;
    
    public AuthorizationWindow()
    {
        userWindow = new UserWindow();
        messageWindow = new MessageWindow();
        _auth = new UserAuth();
        _context = new MkarpovDe092025Context();
        InitializeComponent();
    }

    private void AuthButtonOnClick(object? sender, RoutedEventArgs e)
    {
        var user = new User();
        
        try
        {
            user = _context.Users.First(
                u => u.Login == _auth.Login && u.Password == _auth.Password );
        }
        catch (Exception)
        {
            messageWindow.Message.Text = 
                "Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные";
            messageWindow.Activate();
        }
        
        messageWindow.Message.Text = "Вы успешно авторизовались";
        messageWindow.Show();
        
        
        
    }
}