using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;

namespace app;

public partial class App : Application
{
    private IConfiguration _configuration;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        /*ConfigureAppSettings();*/
    }
    
    private void ConfigureAppSettings()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ConfigureAppSettings();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new Windows.AuthorizationWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}