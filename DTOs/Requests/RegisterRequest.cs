using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Requests
{
    public class RegisterRequest : UpdateUserRequest
    {
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    
    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only digits.")]
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string RegionId { get; set; } = null!;
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public string Street { get; set; } = null!;
        public IFormFile? Picture { get; set; }
    }
}
