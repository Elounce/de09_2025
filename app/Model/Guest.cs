using System;
using System.Collections.Generic;

namespace app.Model;

public partial class Guest
{
    public int GuestId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int RoomId { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime DepartureDate { get; set; }

    public virtual Room Room { get; set; } = null!;
}
