using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCart { get; set; } = true;
        [JsonIgnore]
        public ICollection<Item>? Items { get; set; }
        [ForeignKey(nameof(Payment))]
        public string? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        [ForeignKey(nameof(Voucher))]
        public Guid? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }   // customer
        public User? User { get; set; }
        [ForeignKey(nameof(ShippingMethod))]
        public string? ShippingMethodId { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }

        public void IncreaseTotalPrice(int quantity, decimal price)
        {
            this.TotalPrice += quantity * price;
        }
    }
}
