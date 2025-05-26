namespace DTOs.Requests
{
    public record ItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
