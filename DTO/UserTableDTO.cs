using System.ComponentModel.DataAnnotations;
using testAPIs.Models;

namespace testAPIs.DTO
{
    public class UserTableDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public long PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string? Passport { get; set; }

        [MinLength(16)]
        public string? AadharNumber { get; set; }

        public virtual UserAddressTableDTO Address { get; set; } = null!;

        //public DateTime CreatedDate { get; set; }

        //public string? Role { get; set; }

    }
    public class CreateUserTableDTO : UserTableDTO
    {
        public string Password { get; set; } = null!;

    }

    public class UpdateUserTableDTO : UserTableDTO
    {
        public int Id { get; set; }

    }

    public class GetUserTableDTO : UserTableDTO
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Role { get; set; }
    }
}
