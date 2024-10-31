using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;

namespace app.Windows;

public partial class UserWindow : Window
{
    public UserWindow()
    {
        InitializeComponent();
        ContentTemplate = (DataTemplate)Resources["CustomerTemplate"]!;
    }

    private void ChangePassword_OnClick(object? sender, RoutedEventArgs e)
    {
        /*var curPass = Current
        
        if ()
        {
            
        }*/
        ContentTemplate = (DataTemplate)Resources["AdministratorTemplate"]!;
    }
}