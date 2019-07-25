using System;

namespace MassTransitDemo.CourierDemo.Activities.ApplyCoupon
{
    public class ApplyCouponArguments
    {
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }

        public string CouponCode { get; set; }
    }
}