using System.Threading.Tasks;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.CourierDemo.Activities.ApplyCoupon
{
    public class ApplyCouponActivity : ExecuteActivity<ApplyCouponArguments>
    {
        private readonly ILogger<ApplyCouponActivity> logger;

        public ApplyCouponActivity(ILogger<ApplyCouponActivity> logger)
        {
            this.logger = logger;
        }

        public Task<ExecutionResult> Execute(ExecuteContext<ApplyCouponArguments> context)
        {
            if (context.Arguments.Quantity < 5 )
            {
                return Task.FromResult(context.Faulted());
            }

            this.logger.LogInformation($"Applied discount of $10");
            return Task.FromResult(context.CompletedWithVariables(new { Discount = 10 }));
        }
    }
}