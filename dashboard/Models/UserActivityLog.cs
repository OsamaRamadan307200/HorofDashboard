using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class UserActivityLog
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public string ActivityType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
