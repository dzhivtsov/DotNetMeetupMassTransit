using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransitDemo.Shared.Commands
{
    public interface IPlaceOrder
    {
        string ProductName { get; set; }

        int Quantity { get; set; }
    }
}
