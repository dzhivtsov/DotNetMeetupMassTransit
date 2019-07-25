using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.Shared.Commands;
using MassTransitDemo.Web.Controllers.Models;
using MassTransitDemo.Web.Events;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitDemo.Web.Controllers
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
                ProductName = model.ProductName,
                Quantity = model.Quantity,
                OrderId = orderId
            });

            return orderId;
        }
    }
}
