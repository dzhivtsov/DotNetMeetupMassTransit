using System;

namespace MassTransitDemo.Shared.Events
{
   public interface IOrderSubmitted
    {
        string ProductName { get; set; }

        int Quantity { get; set; }

        Guid OrderId { get; set; }
    }
}
