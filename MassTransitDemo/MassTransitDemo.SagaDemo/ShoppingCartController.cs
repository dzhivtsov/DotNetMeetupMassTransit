using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.SagaDemo.Events;
using MassTransitDemo.SagaDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitDemo.SagaDemo
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public ShoppingCartController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public Task AddItem([FromBody] ItemModel model)
        {
            return this.publishEndpoint.Publish(new CartItemAdded
            {
                Username = model.Username,
                Timestamp = DateTime.Now
            });
        }

        [HttpPost]
        public Task CheckOut([FromBody]CheckOutModel model)
        {
            return this.publishEndpoint.Publish(new CartCheckedOut
            {
                Username = model.Username,
                Timestamp = DateTime.Now
            });
        }
    }
}
