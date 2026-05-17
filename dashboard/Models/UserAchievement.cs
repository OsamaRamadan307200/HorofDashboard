using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class UserAchievement
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int AchievementId { get; set; }

    public DateTime EarnedAt { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
