using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class Level
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public int? Position { get; set; }

    public string? PicIcon { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Url { get; set; } = null!;

    public string? Features { get; set; }

    public bool IsActive { get; set; }

    public decimal Price { get; set; }

    public bool RequiresPayment { get; set; }

    public virtual ICollection<LevelPricing> LevelPricings { get; set; } = new List<LevelPricing>();

    public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();

    public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
