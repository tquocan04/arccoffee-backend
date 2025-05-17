using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class District
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public ICollection<Address>? Addresses { get; set; }
        [ForeignKey(nameof(City))]
        public Guid CityId { get; set; }
        public City? City { get; set; }
    }
}
