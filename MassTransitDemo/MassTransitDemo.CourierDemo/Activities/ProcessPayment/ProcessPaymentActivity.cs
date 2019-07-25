using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.CourierDemo.Activities.ProcessPayment
{
    public class ProcessPaymentActivity : Activity<ProcessPaymentArguments, ProcessPaymentLog>
    {
        private readonly ILogger<ProcessPaymentActivity> logger;

        public ProcessPaymentActivity(ILogger<ProcessPaymentActivity> logger)
        {
            this.logger = logger;
        }

        public Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArguments> context)
        {
            Guid paymentId = Guid.NewGuid();
            this.logger.LogInformation(
                $"Payment for order {context.Arguments.OrderId} has been processed with discount of {context.Arguments.Discount}. Payment id {paymentId}");
            Console.WriteLine("Log");
            return Task.FromResult(context.Completed<ProcessPaymentLog>(new { PaymentId = paymentId }));
        }

        public Task<CompensationResult> Compensate(CompensateContext<ProcessPaymentLog> context)
        {
            this.logger.LogInformation($"Payment {context.Log.PaymentId} has been cancelled");
            return Task.FromResult(context.Compensated());
        }
    }
}