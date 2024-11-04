using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace app.Windows;

public partial class NewUserWindow : Window
{
    public IEnumerable<string> RolesList { get; private set; }
    
    public NewUserWindow(IEnumerable<string> roles)
    {
        RolesList = roles;
        InitializeComponent();
        DataContext = this;
    }

    private async Task PostUser(User user)
    {
        await using (var context = new MkarpovDe092025Context())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }

    private async Task<bool> IsUserExist(string login)
    {
        await using (var context = new MkarpovDe092025Context())
        {
            var res = context.Users.FirstOrDefault(u => u.Login == login);
            return res != null;
        }
    }
    
    private async Task OpenMessageWindow(string message)
    {
        var msgWindow = new MessageWindow();
        msgWindow.Message.Text = message;
        await msgWindow.ShowDialog<bool>(this);
    }

    private async void AddNewUser_OnClick(object? sender, RoutedEventArgs e)
    {
        var log = LoginBox.Text;
        var pass = PassBox.Text;
        var fName = FirstNameBox.Text;
        var lName = LastNameBox.Text;
        var roleId = RoleComboBox.SelectedIndex;

        if (string.IsNullOrWhiteSpace(log) || string.IsNullOrWhiteSpace(pass) || roleId == 0)
        {
            await OpenMessageWindow("Заполните необходимые поля");
            return;
        }
        
        var user = new User
        {
            Login = log,
            Password = pass,
            RoleId = roleId,
            FirstName = fName,
            LastName = lName
        };

        if (await IsUserExist(log))
        {
            await OpenMessageWindow("Пользователь с таким логином уже есть");
            return;
        }
   
        await PostUser(user);
        await OpenMessageWindow($"Добавлен новый пользователь с логином {log}");
        Close(this);
    }
}