using System;

namespace MassTransitDemo.SagaDemo.Events
{
    public class PaymentProcessed
    {
        public string Username { get; set; }

        public Guid CartId { get; set; }
    }
}