using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class LevelPricing
{
    public int Id { get; set; }

    public int LevelId { get; set; }

    public string PriceType { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public decimal? DiscountedAmount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Level Level { get; set; } = null!;
}
