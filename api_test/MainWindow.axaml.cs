using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using api_test.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace api_test;

public partial class MainWindow : Window
{
    private string? result = "";
    private readonly List<Char> _forbidenSybmols = ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '_'];
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void GetName_OnClick(object? sender, RoutedEventArgs e)
    {
        HttpClient client = new HttpClient();

        using (client)
        {
            var response = await client.GetFromJsonAsync<Name>("http://prb.sylas.ru/TransferSimulator/fullName");
            if(response != null)
                result = response.Value;
        }
        
        NameTextBlock.Text = result;
    }

    private void CheckName_OnClick(object? sender, RoutedEventArgs e)
    {
        /*foreach (char c in _forbidenSybmols)
        {
            if (result.Contains(c))
            {
                CheckResult.Text = "ФИО содержит запрещённые символы";
                return; 
            }
        }*/

        var chars = result.ToCharArray();
        foreach (char c in chars)
        {
            if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
            {
                CheckResult.Text = "ФИО содержит запрещённые символы";
                return; 
            }
        }

        CheckResult.Text = "ФИО не содержит запрещённые символы";
    }
}