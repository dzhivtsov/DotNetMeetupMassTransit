using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.Shared.Events;

namespace MassTransitDemo.OrderProcessor
{
    internal class OrderSubmittedConsumer : IConsumer<IOrderSubmitted>
    {
        private readonly IWarehouseService warehouseService;
        
        public OrderSubmittedConsumer(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            await this.warehouseService.ReserveProducts(context.Message.ProductName, context.Message.Quantity);

            Console.WriteLine($"Order {context.Message.OrderId} reserved at {DateTime.Now.ToLongTimeString()}");

            await context.Publish<IOrderProcessed>(new OrderProcessedEventArgs(context.Message.OrderId));
        }
    }
}
