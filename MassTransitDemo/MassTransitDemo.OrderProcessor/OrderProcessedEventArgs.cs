using System;
using MassTransitDemo.Shared.Events;

namespace MassTransitDemo.OrderProcessor
{
    internal class OrderProcessedEventArgs : IOrderProcessed
    {
        public Guid OrderId { get; }

        public OrderProcessedEventArgs(Guid orderId)
        {
            this.OrderId = orderId;
        }
    }
}
