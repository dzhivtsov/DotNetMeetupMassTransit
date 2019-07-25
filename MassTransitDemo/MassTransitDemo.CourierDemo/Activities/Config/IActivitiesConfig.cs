namespace MassTransitDemo.CourierDemo.Activities.Config
{
    public interface IActivitiesConfig
    {
        IActivityConfig ReserveProduct { get; }

        IActivityConfig ProcessPayment { get; }

        IActivityConfig ApplyCoupon { get; }
    }
}