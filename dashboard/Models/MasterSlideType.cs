using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class MasterSlideType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? TypeClassName { get; set; }

    public virtual ICollection<Slide> Slides { get; set; } = new List<Slide>();
}
