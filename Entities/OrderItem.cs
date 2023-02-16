namespace MovieAPi.Entities
{
    public class OrderItem : AuditableBaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int MovieScheduleId { get; set; }
        public MovieSchedule MovieSchedule { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double SubTotalPrice { get; set; }
        public string SnapShots { get; set; }
    }
}