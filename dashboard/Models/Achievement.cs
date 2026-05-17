using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public string BadgeColor { get; set; } = null!;

    public string RequirementType { get; set; } = null!;

    public int RequirementValue { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
