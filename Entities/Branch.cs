using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Branch
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [Range(9, 11)]
        public string? PhoneNumber { get; set; }
        [JsonIgnore]
        public ICollection<BranchProduct>? BranchProducts { get; set; }
        public ICollection<User>? Users { get; set; }
        [JsonIgnore]
        public ICollection<Address>? Addresses { get; set; }
    }
}
