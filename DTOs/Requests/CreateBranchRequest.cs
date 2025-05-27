using System.ComponentModel.DataAnnotations;

namespace DTOs.Requests
{
    public record CreateBranchRequest
    {
        public string? Name { get; init; }
        [MaxLength(11)]
        public string? PhoneNumber { get; init; }
        public string RegionId { get; init; } = null!;
        public Guid CityId { get; init; }
        public Guid DistrictId { get; init; }
        public string Street { get; init; } = null!;
    }
}
