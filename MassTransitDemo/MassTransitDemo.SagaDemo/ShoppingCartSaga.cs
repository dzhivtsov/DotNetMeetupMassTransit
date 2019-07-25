using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransitDemo.SagaDemo.Events;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.SagaDemo
{
    public class ProcessPaymentConsumer : IConsumer<ProcessPayment>
    {
        private readonly ILogger<ProcessPaymentConsumer> logger;

        public ProcessPaymentConsumer(ILogger<ProcessPaymentConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<ProcessPayment> context)
        {
            this.logger.LogInformation(
                $"Processing payment for user {context.Message.Username}, cart {context.Message.CartId}");
            return context.Publish(new PaymentProcessed
                {CartId = context.Message.CartId, Username = context.Message.Username});
        }
    }
}
