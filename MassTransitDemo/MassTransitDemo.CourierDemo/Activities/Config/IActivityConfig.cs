using System;

namespace MassTransitDemo.CourierDemo.Activities.Config
{
    public interface IActivityConfig
    {
        string ExecuteQueueName { get; }

        string CompensateQueueName { get; }

        Uri ExecuteEndpointUri { get; }

        Uri CompensateEndpointUri { get; }
        string ActivityName { get; }
    }
}