using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class ApplicationUser
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? ProfilePicture { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Country { get; set; }

    public string PreferredLanguage { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; }

    public int TotalStudyTime { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();

    public virtual ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();

    public virtual UserProgress? UserProgress { get; set; }

    public virtual ICollection<UserSlideProgress> UserSlideProgresses { get; set; } = new List<UserSlideProgress>();

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
