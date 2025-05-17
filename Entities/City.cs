using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class City
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [ForeignKey(nameof(Region))]
        public string RegionId { get; set; } = null!;
        public Region? Region { get; set; }
        public ICollection<District>? Districts { get; set; }
    }
}
