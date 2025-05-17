namespace Entities
{
    public class ShippingMethod
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public ICollection<Order>? Oders { get; set; }
    }
}
