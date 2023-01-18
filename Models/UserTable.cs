using System;
using System.Collections.Generic;

namespace testAPIs.Models;

public partial class UserTable
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public long PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Passport { get; set; }

    public long? AadharNumber { get; set; }

    public DateTime CreatedDate { get; set; }
}

public class LogInReqBody
{
    public string email { get; set; } = null!;

    public string password { get; set; } = null!;
}

