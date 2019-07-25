using System;

namespace MassTransitDemo.CourierDemo.Activities.ProcessPayment
{
    public class ProcessPaymentArguments
    {
        public Guid OrderId { get; set; }

        public decimal? Discount { get; set; }
    }
}