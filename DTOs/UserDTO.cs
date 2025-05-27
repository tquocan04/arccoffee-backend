namespace DTOs
{
    public class AllUserDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string RegionId { get; set; } = null!;
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public string Street { get; set; } = null!;
    }
    public class UserDTO : AllUserDTO
    {
        public string? Picture { get; set; }
    }

    public class UserUpdateDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly Dob { get; set; }
        public string? Picture { get; set; }
        public string RegionId { get; set; } = null!;
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public string Street { get; set; } = null!;
    }

    public class StaffDTO : AllUserDTO
    {
        public string Role { get; set; } = null!;
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? RegionName { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
    }
}
