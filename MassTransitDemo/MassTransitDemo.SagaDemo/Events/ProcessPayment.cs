using System;

namespace MassTransitDemo.SagaDemo.Events
{
    public class ProcessPayment
    {
        public string Username { get; set; }

        public Guid CartId { get; set; }
    }
}