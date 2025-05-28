namespace DTOs.Requests
{
    public record BillRequest
    {
        public string? PaymentId { get; init; }
        public string? ShippingId { get; init; }
        public string? VoucherCode { get; init; }
        public string? PhoneNumber { get; init; }
        public string RegionId { get; init; } = null!;
        public Guid CityId { get; init; }
        public Guid DistrictId { get; init; }
        public string Street { get; init; } = null!;
        public string? Note { get; init; }
    }
}
