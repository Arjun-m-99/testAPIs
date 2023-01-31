using System;
using System.Collections.Generic;

namespace testAPIs.Models;

public partial class UserActivityTable
{
    public int RowNumber { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string GeneratedToken { get; set; } = null!;

    public DateTime TimeDate { get; set; }

    public string? Ipaddress { get; set; }

    public virtual UserTable User { get; set; } = null!;
}
