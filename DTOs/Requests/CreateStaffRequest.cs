using System.ComponentModel.DataAnnotations;

namespace DTOs.Requests
{
    public record CreateStaffRequest
    {
        public string? Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [MaxLength(12)]
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string RegionId { get; set; } = null!;
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public string Street { get; set; } = null!;
        public Guid BranchId { get; set; }
    }
}
