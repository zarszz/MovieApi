namespace MovieAPi.Entities
{
    public class Studio : AuditableBaseEntity
    {
        public int StudioNumber { get; set; }
        public int SeatCapacity { get; set; }
    }
}