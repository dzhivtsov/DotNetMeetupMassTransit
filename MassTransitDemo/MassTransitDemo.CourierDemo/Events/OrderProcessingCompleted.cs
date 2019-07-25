using System;

namespace MassTransitDemo.CourierDemo.Events
{
    public class OrderProcessingCompleted
    {
        public Guid OrderId { get; set; }
    }
}