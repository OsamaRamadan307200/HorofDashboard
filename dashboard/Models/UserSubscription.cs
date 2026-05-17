using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class UserSubscription
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int LevelId { get; set; }

    public string SubscriptionType { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public virtual Level Level { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
