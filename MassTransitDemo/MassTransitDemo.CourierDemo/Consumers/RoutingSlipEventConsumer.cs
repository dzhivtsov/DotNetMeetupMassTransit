using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.CourierDemo.Consumers
{
    public class RoutingSlipEventConsumer : IConsumer<RoutingSlipFaulted>
    {
        private readonly ILogger<RoutingSlipEventConsumer> logger;

        public RoutingSlipEventConsumer(ILogger<RoutingSlipEventConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            this.logger.LogInformation($"Routing slip {context.Message.TrackingNumber} faulted.");
            return Task.CompletedTask;
        }
    }
}