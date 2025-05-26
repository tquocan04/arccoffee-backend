namespace DTOs
{
    public record PaymentDTO
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
}
