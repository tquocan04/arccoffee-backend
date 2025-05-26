namespace DTOs.Requests
{
    public record CreateVoucherRequest
    {
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public int Percentage { get; set; }
        public decimal MaxDiscount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal MinOrderValue { get; set; }
        public int Quantity { get; set; }
    }
}
