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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}