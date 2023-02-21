using System.Collections.Generic;

namespace MovieAPi.DTOs.V1.Request
{
    public class CreateOrderDto     
    {
        public string PaymentMethod { get; set; }
        public List<SingleCreateOrderDto> OrderItems { get; set; }
    }
    
    public class SingleCreateOrderDto
    {
        public int MovieScheduleId { get; set; }
        public int Qty { get; set; }
    }
}