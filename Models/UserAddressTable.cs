using System;
using System.Collections.Generic;

namespace testAPIs.Models;

public partial class UserAddressTable
{
    public int RowNumber { get; set; }

    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? AddressLine3 { get; set; }

    public string? StateName { get; set; }

    public string? CountryName { get; set; }

    public int? Zipcode { get; set; }

    public DateTime AddedTime { get; set; }

    public virtual UserTable IdNavigation { get; set; } = null!;
}
