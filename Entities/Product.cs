using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Image { get; set; }
        public bool IsAvailable { get; set; } = true;
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        [JsonIgnore]
        public ICollection<BranchProduct>? BranchProducts { get; set; }
        [JsonIgnore]
        public ICollection<Item>? Items { get; set; }

        public void ChangeStatus()
        {
            if (IsAvailable)
                IsAvailable = false;
            else
                IsAvailable = true;
        }
    }
}
