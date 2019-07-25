using System;

namespace MassTransitDemo.SagaDemo.Events
{
    public class CartItemAdded
    {
        public string Username { get; set; }

        public DateTime Timestamp { get; set; }
    }
}