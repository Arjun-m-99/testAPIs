namespace testAPIs.DTO
{
    public class UserAddressTableDTO
    {
        //public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? AddressLine3 { get; set; }

        public string? StateName { get; set; }

        public string? CountryName { get; set; }

        public int? Zipcode { get; set; }

        //public DateTime AddedTime { get; set; }

    }
}
