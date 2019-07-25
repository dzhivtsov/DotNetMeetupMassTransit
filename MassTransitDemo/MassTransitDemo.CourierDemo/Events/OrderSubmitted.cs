using System;

namespace MassTransitDemo.CourierDemo.Events
{
    public class OrderSubmitted
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }

        public string CouponCode { get; set; }
    }
}