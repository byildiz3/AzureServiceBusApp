using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceApp.ProducerAPI.Services;
using ServiceBusApp.Common;
using ServiceBusApp.Common.Dto;
using ServiceBusApp.Common.Events;

namespace ServiceApp.ProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AzureServices _azureServices;
        public OrderController(AzureServices azureServices)
        {
            _azureServices= azureServices;
        }
        [HttpPost]
        public async Task CreateOrder(OrderDto order)
        {
            var orderCreatedEvent = new OrderCreatedEvent()
            {
                Id = order.Id,
                ProductName = order.ProductName,
                CreatedOn = DateTime.UtcNow,
                
            };

            await _azureServices.SendMessageToTopic(Constans.OrderTopic, orderCreatedEvent,Constans.OrderCreatedSubscription,"OrderCreated","OrderCreatedOnly");
        }

        [HttpDelete("id")]
        public async Task DeleteOrder(int id)
        {

            var orderDeletedEvent = new OrderDeletedEvent()
            {
                Id = id, 
                CreatedOn = DateTime.UtcNow,

            };
            await _azureServices.SendMessageToTopic(Constans.OrderTopic, orderDeletedEvent, Constans.OrderDeletedSubscription, "OrderDeleted", "OrderDeletedOnly");
        }
    }
}
