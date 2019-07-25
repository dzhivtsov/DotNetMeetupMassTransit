using System;

namespace MassTransitDemo.CourierDemo.Activities.ReserveProduct
{
    public class ReserveProductArguments
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
    }
}