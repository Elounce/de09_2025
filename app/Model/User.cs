using System;
using System.Collections.Generic;

namespace app.Model;

public partial class User
{
    public int UserId { get; set; }

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Login { get; set; } = null!;

    public DateTime? LastLoginDate { get; set; }

    public int? LoginAttempts { get; set; }

    public virtual Role Role { get; set; } = null!;
}
