using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class PaymentTransaction
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int LevelId { get; set; }

    public string TransactionId { get; set; } = null!;

    public string PaymentProvider { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? PaymentIntentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public virtual Level Level { get; set; } = null!;

    public virtual ApplicationUser User { get; set; } = null!;
}
