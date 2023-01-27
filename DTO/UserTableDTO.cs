namespace testAPIs.DTO
{
    public class UserTableDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public long PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string? Passport { get; set; }

        public long? AadharNumber { get; set; }

        //public DateTime CreatedDate { get; set; }

        //public string? Role { get; set; }

    }
    public class CreateUserTableDTO : UserTableDTO
    {
        public string Password { get; set; } = null!;

    }

    public class UpdateUserTableDTO : CreateUserTableDTO
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
