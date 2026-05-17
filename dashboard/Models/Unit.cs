using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class Unit
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public int Position { get; set; }

    public DateTime CreatedAt { get; set; }

    public int LevelId { get; set; }

    public string Url { get; set; } = null!;

    public int? HoursWork { get; set; }

    public int? LessonsCount { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Level Level { get; set; } = null!;

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
}
