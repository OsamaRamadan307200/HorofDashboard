using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class UserProgress
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int? LevelId { get; set; }

    public int? UnitId { get; set; }

    public int? LessonId { get; set; }

    public int? SlideId { get; set; }

    public int CurrentLevel { get; set; }

    public int CurrentUnit { get; set; }

    public int CurrentLesson { get; set; }

    public int CurrentSlide { get; set; }

    public decimal OverallProgressPercent { get; set; }

    public DateTime LastAccessedAt { get; set; }

    public int StudyStreak { get; set; }

    public DateTime? LastStudyDate { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual Level? Level { get; set; }

    public virtual Slide? Slide { get; set; }

    public virtual Unit? Unit { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;
}
