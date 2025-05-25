namespace DTOs
{
    public class ItemDTO
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
    }
}
