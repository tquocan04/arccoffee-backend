namespace DTOs
{
    public class BranchDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string RegionId { get; set; } = null!;
        public string? RegionName { get; set; }
        public Guid CityId { get; set; }
        public string? CityName { get; set; }
        public Guid DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string Street { get; set; } = null!;
    }
}
