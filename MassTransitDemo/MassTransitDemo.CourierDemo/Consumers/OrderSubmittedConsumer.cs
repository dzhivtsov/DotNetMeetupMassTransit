using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransitDemo.CourierDemo.Activities.Config;
using MassTransitDemo.CourierDemo.Events;

namespace MassTransitDemo.CourierDemo.Consumers
{
    public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
    {
        private readonly IActivitiesConfig activitiesConfig;

        public OrderSubmittedConsumer(IActivitiesConfig activitiesConfig)
        {
            this.activitiesConfig = activitiesConfig;
        }

        public Task Consume(ConsumeContext<OrderSubmitted> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity(this.activitiesConfig.ReserveProduct.ActivityName,
                this.activitiesConfig.ReserveProduct.ExecuteEndpointUri,
                new
                {
                    context.Message.Quantity,
                    context.Message.ProductName
                });

            if (!string.IsNullOrWhiteSpace(context.Message.CouponCode))
            {
                builder.AddActivity(this.activitiesConfig.ApplyCoupon.ActivityName,
                    this.activitiesConfig.ApplyCoupon.ExecuteEndpointUri,
                    new {context.Message.CouponCode, context.Message.Quantity});
            }

            builder.AddActivity(this.activitiesConfig.ProcessPayment.ActivityName,
                this.activitiesConfig.ProcessPayment.ExecuteEndpointUri);

            builder.AddVariable(nameof(OrderSubmitted.OrderId), context.Message.OrderId);

            builder.AddSubscription(new Uri("rabbitmq://localhost/orders"),
                RoutingSlipEvents.Completed,
                sendEndpoint => sendEndpoint.Send(new OrderProcessingCompleted
                {
                    OrderId = context.Message.OrderId
                }));

            builder.AddSubscription(new Uri("rabbitmq://localhost/orders"),
                RoutingSlipEvents.Faulted);

            RoutingSlip routingSlip = builder.Build();
            
            return context.Execute(routingSlip);
        }
    }
}