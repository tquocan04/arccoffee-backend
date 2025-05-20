namespace DTOs
{
    public class RegionDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public ICollection<CityDTO>? Cities { get; set; }
    }

    public class CityDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<DistrictDTO>? Districts { get; set; }
    }

    public class DistrictDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
