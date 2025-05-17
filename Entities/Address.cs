using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey(nameof(Branch))]
        public Guid? BranchId { get; set; }
        public Branch? Branch { get; set; }
        [ForeignKey(nameof(District))]
        public Guid DistrictId { get; set; }
        public District? District { get; set; }
        public string Street { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
