using System;
using Automatonymous;

namespace MassTransitDemo.SagaDemo.Saga
{
    public class ShoppingCart : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public string UserName { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public Guid? ExpirationId { get; set; }
    }
}