using System;
using System.Collections.Generic;

namespace app.Model;

public partial class Floor
{
    public int FloorId { get; set; }

    public string FloorNumber { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
