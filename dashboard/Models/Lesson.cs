using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Details { get; set; }

    public bool Status { get; set; }

    public int? MinutesWork { get; set; }

    public int Position { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UnitId { get; set; }

    public string Url { get; set; } = null!;

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual ICollection<Slide> Slides { get; set; } = new List<Slide>();

    public virtual Unit Unit { get; set; } = null!;

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
}
