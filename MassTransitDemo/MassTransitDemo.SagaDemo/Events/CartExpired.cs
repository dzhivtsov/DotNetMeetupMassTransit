using System;

namespace MassTransitDemo.SagaDemo.Events
{
    public class CartExpired
    {
        public Guid CartId { get; set; }
    }
}