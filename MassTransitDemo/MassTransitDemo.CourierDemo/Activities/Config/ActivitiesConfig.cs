using MassTransitDemo.CourierDemo.Activities.ApplyCoupon;
using MassTransitDemo.CourierDemo.Activities.ProcessPayment;
using MassTransitDemo.CourierDemo.Activities.ReserveProduct;

namespace MassTransitDemo.CourierDemo.Activities.Config
{
    public class ActivitiesConfig : IActivitiesConfig
    {
        public ActivitiesConfig(string baseUri, string baseQueueName)
        {
            this.ReserveProduct = new ActivityConfig<ReserveProductActivity>(baseUri, baseQueueName);
            this.ApplyCoupon = new ActivityConfig<ApplyCouponActivity>(baseUri, baseQueueName);
            this.ProcessPayment = new ActivityConfig<ProcessPaymentActivity>(baseUri, baseQueueName);
        }

        public IActivityConfig ReserveProduct { get; }

        public IActivityConfig ApplyCoupon { get; }

        public IActivityConfig ProcessPayment { get; }
    }
}