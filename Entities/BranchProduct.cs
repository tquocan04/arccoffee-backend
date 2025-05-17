using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class BranchProduct
    {
        [ForeignKey(nameof(Branch))]
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public int? Quantity { get; set; }
    }
}
