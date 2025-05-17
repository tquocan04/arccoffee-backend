using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class User : IdentityUser<string>
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly Dob { get; set; }
        public string? Picture { get; set; }
        public string? GoogleId { get; set; }
        [JsonIgnore]
        public ICollection<Address>? Addresses { get; set; }
        [ForeignKey(nameof(Branch))]
        public Guid? BranchId { get; set; } // for staff and admin
        public Branch? Branch { get; set; }
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}
