using System;
using System.Collections.Generic;

namespace app.Model;

public partial class Room
{
    public int RoomId { get; set; }

    public int FloorId { get; set; }

    public int Number { get; set; }

    public int CategoryId { get; set; }

    public int StatusId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Cleaning? Cleaning { get; set; }

    public virtual Floor Floor { get; set; } = null!;

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual Status Status { get; set; } = null!;
}
