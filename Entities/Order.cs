using System.Collections.Generic;

namespace MovieAPi.Entities
{
    public class Order : AuditableBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalItemPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}