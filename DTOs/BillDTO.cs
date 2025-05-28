namespace DTOs
{
    public class BillDTO
    {
        public Guid Id { get; set; }
        public ICollection<ItemDTO>? Items { get; set; }
        public string? CustomerId { get; set; }
        public string? Name { get; set; }
        public string? ShippingId { get; set; }
        public string? Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PaymentId { get; set; }
        public Guid? VoucherId { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
