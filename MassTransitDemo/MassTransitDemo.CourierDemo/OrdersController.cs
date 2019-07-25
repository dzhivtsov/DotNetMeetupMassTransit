using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.CourierDemo.Events;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitDemo.CourierDemo
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public OrdersController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<Guid> Post([FromBody]OrderModel model)
        {
            Guid orderId = NewId.NextGuid();

            await this.publishEndpoint.Publish(new OrderSubmitted
            {
                OrderId = orderId,
                Quantity = model.Quantity,
                ProductName = model.ProductName,
                CouponCode = model.CouponCode
            });

            return orderId;
        }
    }

    public class OrderModel
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public string CouponCode { get; set; }
    }
}
