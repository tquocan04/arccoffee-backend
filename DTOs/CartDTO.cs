namespace DTOs
{
    public class CartDTO
    {
        public ICollection<ItemDTO>? Items { get; set; }
        public decimal TotalPrice { get; set; } = 0;
    }
}
