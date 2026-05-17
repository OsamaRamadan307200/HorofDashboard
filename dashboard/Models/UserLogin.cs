using System;
using System.Collections.Generic;

namespace HorofDashboard.Models;

public partial class UserLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    public string? UserId { get; set; }
}
