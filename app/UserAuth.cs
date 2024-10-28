using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Markup;
using Avalonia.Remote.Protocol;

namespace app;

public class UserAuth
{
    [Required]
    [Display(Name = "Login")]
    public string Login { get; set; }
    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}