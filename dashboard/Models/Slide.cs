using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class Slide
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public int? Position { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? SlideContent { get; set; }

    public string? Answer { get; set; }

    public int? Points { get; set; }

    public int? MasterSlideTypeId { get; set; }

    public int LessonId { get; set; }

    public Guid? UrlId { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual MasterSlideType? MasterSlideType { get; set; }

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();

    public virtual ICollection<UserSlideProgress> UserSlideProgresses { get; set; } = new List<UserSlideProgress>();
}
