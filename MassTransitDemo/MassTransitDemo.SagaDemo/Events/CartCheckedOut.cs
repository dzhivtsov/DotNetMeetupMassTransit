using System;

namespace MassTransitDemo.SagaDemo.Events
{
    public class CartCheckedOut
    {
        public string Username { get; set; }

        public DateTime Timestamp { get; set; }
    }
}