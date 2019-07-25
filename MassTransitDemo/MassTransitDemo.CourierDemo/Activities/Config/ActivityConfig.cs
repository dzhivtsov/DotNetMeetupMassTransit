using System;

namespace MassTransitDemo.CourierDemo.Activities.Config
{
    public class ActivityConfig<TActivity> : IActivityConfig
    {
        private readonly string baseUri;

        private readonly string baseQueueName;

        public string ActivityName => typeof(TActivity).Name.Replace("Activity", string.Empty);

        public string ExecuteQueueName => $"{this.baseQueueName}-{this.ActivityName}";

        public string CompensateQueueName => $"{this.ExecuteQueueName}-compensate";

        public Uri ExecuteEndpointUri => new Uri($"{baseUri}/{this.ExecuteQueueName}");

        public Uri CompensateEndpointUri => new Uri($"{baseUri}/{this.CompensateQueueName}");

        public ActivityConfig(string baseUri, string baseQueueName)
        {
            this.baseUri = baseUri;
            this.baseQueueName = baseQueueName;
        }
    }
}