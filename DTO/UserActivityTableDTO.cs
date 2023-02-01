using testAPIs.Models;

namespace testAPIs.DTO
{
    public class UserActivityTableDTO
    {
        public int RowNumber { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string GeneratedToken { get; set; } = null!;
    }
    public class GetUserActivityTableDTO : UserActivityTableDTO
    {
        public DateTime TimeDate { get; set; }

        public string? Ipaddress { get; set; }

        //public virtual UserTable User { get; set; } = null!;
    }
}
