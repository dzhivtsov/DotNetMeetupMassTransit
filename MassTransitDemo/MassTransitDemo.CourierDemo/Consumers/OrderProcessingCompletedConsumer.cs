using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.CourierDemo.Events;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.CourierDemo.Consumers
{
    public class OrderProcessingCompletedConsumer :IConsumer<OrderProcessingCompleted>
    {
        private readonly ILogger<OrderProcessingCompletedConsumer> logger;

        public OrderProcessingCompletedConsumer(ILogger<OrderProcessingCompletedConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<OrderProcessingCompleted> context)
        {
            this.logger.LogInformation($"Processing of order {context.Message.OrderId} has completed.");
            return Task.CompletedTask;
        }
    }
}