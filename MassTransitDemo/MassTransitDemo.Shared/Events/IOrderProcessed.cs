using System;

namespace MassTransitDemo.Shared.Events
{
    public interface IOrderProcessed
    {
        Guid OrderId { get; }
    }
}
