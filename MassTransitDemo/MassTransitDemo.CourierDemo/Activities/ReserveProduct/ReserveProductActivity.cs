using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.CourierDemo.Activities.ReserveProduct
{
    public class ReserveProductActivity : Activity<ReserveProductArguments, ReserveProductLog>
    {
        private readonly ILogger<ReserveProductActivity> logger;

        public ReserveProductActivity(ILogger<ReserveProductActivity> logger)
        {
            this.logger = logger;
        }

        public Task<ExecutionResult> Execute(ExecuteContext<ReserveProductArguments> context)
        {
            Guid reservationId = Guid.NewGuid();
            this.logger.LogInformation(
                $"Order {context.Arguments.OrderId} reserved ({context.Arguments.ProductName}, {context.Arguments.Quantity}. Reservation id {reservationId}");
            return Task.FromResult(context.Completed<ReserveProductLog>(new {ReservationId = reservationId}));
        }

        public Task<CompensationResult> Compensate(CompensateContext<ReserveProductLog> context)
        {
            this.logger.LogInformation($"Reservation {context.Log.ReservationId} has been cancelled");
            return Task.FromResult(context.Compensated());
        }
    }
}