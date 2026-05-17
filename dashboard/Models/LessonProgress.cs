using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class LessonProgress
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int LessonId { get; set; }

    public bool IsStarted { get; set; }

    public bool IsCompleted { get; set; }

    public int CurrentSlideIndex { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public TimeOnly TotalTimeSpent { get; set; }

    public int Score { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
