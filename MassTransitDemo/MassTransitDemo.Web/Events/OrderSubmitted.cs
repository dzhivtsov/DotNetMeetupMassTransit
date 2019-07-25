using System;
using MassTransitDemo.Shared.Events;

namespace MassTransitDemo.Web.Events
{
    internal class OrderSubmitted : IOrderSubmitted
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
    }
}