using System.Text.Json.Serialization;

namespace Entities
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int Percentage { get; set; }
        public decimal MaxDiscount { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public decimal? MinOrderValue { get; set; }
        public int? Quantity { get; set; }
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }

        public bool IsValidExpireDate()
        {
            return ExpiryDate >= DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
