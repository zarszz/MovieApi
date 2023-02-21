using System.Collections.Generic;
using System.Linq;
using MovieAPi.Entities;

namespace MovieAPi.DTOs.V1.Response
{
    public class ResponseOrderDto
    {
        public string PaymentMethod { get; set; }
        public double TotalItemPrice { get; set; }
        public List<SingleResponseOrderItemDto> OrderItems { get; set; }

        public static ResponseOrderDto FromEntity(Order order)
        {
            return new ResponseOrderDto
            {
                TotalItemPrice = order.TotalItemPrice,
                PaymentMethod = order.PaymentMethod,
                OrderItems = order.OrderItems.Select(SingleResponseOrderItemDto.FromEntity).ToList()
            };
        }
    }

    public class SingleResponseOrderItemDto
    {
        public ResponseMovieScheduleDto MovieSchedule { get; set; }
        public int Qty { get; set; }

        public static SingleResponseOrderItemDto FromEntity(OrderItem orderItem)
        {
            return new SingleResponseOrderItemDto
            {
                Qty = orderItem.Qty,
                MovieSchedule = ResponseMovieScheduleDto.FromEntity(orderItem.MovieSchedule)
            };
        }
    }
}