using System;
using System.Collections.Generic;

namespace app.Model;

public partial class Cleaning
{
    public int RoomId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Task { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
