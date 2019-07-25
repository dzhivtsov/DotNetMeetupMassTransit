using System;
using Automatonymous;
using MassTransitDemo.SagaDemo.Events;
using Microsoft.Extensions.Logging;

namespace MassTransitDemo.SagaDemo.Saga
{
    public class ShoppingCartSaga : MassTransitStateMachine<ShoppingCart>
    {
        public ShoppingCartSaga(ILogger<ShoppingCartSaga> logger)
        {
            this.InstanceState(x => x.CurrentState);

            this.Event(() => this.ItemAdded, x => x
                .CorrelateBy(cart => cart.UserName, context => context.Message.Username)
                .SelectId(context => Guid.NewGuid()));

            this.Event(() => this.CheckedOut,
                x => x.CorrelateBy((cart, context) => cart.UserName == context.Message.Username));

            this.Event(() => this.PaymentProcessed,
                x => x.CorrelateBy((cart, context) => cart.UserName == context.Message.Username));

            this.Schedule(() => this.CartExpired, x => x.ExpirationId, x =>
            {
                x.Delay = TimeSpan.FromSeconds(30);
                x.Received = e => e.CorrelateById(context => context.Message.CartId);
            });

            this.Initially(
                this.When(this.ItemAdded)
                    .Then(context =>
                    {
                        context.Instance.Created = context.Data.Timestamp;
                        context.Instance.Updated = context.Data.Timestamp;
                        context.Instance.UserName = context.Data.Username;
                        logger.LogInformation(
                            $"Created new saga for user {context.Data.Username}, {context.Data.Timestamp}");
                    })
                    .Schedule(this.CartExpired, context => new CartExpired {CartId = context.Instance.CorrelationId})
                    .TransitionTo(this.Active));

            this.During(this.Active,
                this.When(this.ItemAdded)
                    .Then(context =>
                    {
                        if (context.Data.Timestamp > context.Instance.Updated)
                        {
                            context.Instance.Updated = context.Data.Timestamp;
                        }

                        logger.LogInformation($"Updating saga with added item {context.Data.Timestamp}");
                    })
                    .Unschedule(this.CartExpired)
                    .Schedule(this.CartExpired, context => new CartExpired {CartId = context.Instance.CorrelationId}),

                this.When(this.CheckedOut)
                    .Then(context =>
                    {
                        if (context.Data.Timestamp > context.Instance.Updated)
                        {
                            context.Instance.Updated = context.Data.Timestamp;
                        }

                        logger.LogInformation($"Checking out the cart, {context.Data.Timestamp}");
                    })
                    .Unschedule(this.CartExpired)
                    .TransitionTo(this.AwaitingPayment)
                    .Publish(context => new ProcessPayment
                        { Username = context.Instance.UserName, CartId = context.Instance.CorrelationId }),

                this.When(this.CartExpired.Received)
                    .Then(context => logger.LogInformation("Received expired event!"))
                    .Publish(context => new SendPromoCode {Username = context.Instance.UserName})
                    .Finalize()
            );

            During(this.AwaitingPayment,
                this.When(this.PaymentProcessed)
                    .Finalize()
            );

            this.SetCompletedWhenFinalized();
        }

        public State Active { get; set; }

        public State AwaitingPayment { get; set; }

        public Event<CartItemAdded> ItemAdded { get; set; }

        public Event<CartCheckedOut> CheckedOut { get; set; }

        public Event<PaymentProcessed> PaymentProcessed { get; set; }

        public Schedule<ShoppingCart, CartExpired> CartExpired { get; set; }
    }
}