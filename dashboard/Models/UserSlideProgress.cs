using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class UserSlideProgress
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int SlideId { get; set; }

    public bool IsCompleted { get; set; }

    public int? Score { get; set; }

    public int TimeSpent { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int AttemptCount { get; set; }

    public int? BestScore { get; set; }

    public string Answer { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public virtual Slide Slide { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
