using Fawry.OrderItems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Fawry.Controllers
{
    [Route("api/app/order-items")]
    [ApiController]
    public class OrderItemController : AbpController
    {
        private readonly IOrderitemAppService _orderItemAppService;

        public OrderItemController(IOrderitemAppService orderItemAppService)
        {
            _orderItemAppService = orderItemAppService;
        }

        [HttpGet("list/{orderId}")]
        public async Task<PagedResultDto<OrderItemDto>> GetOrderItemsListAsync(int orderId)
        {
            return await _orderItemAppService.GetOrderItemsListAsync(orderId);
        }

        [HttpPost("create")]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItemAsync([FromBody] CreateOrderItemDto input)
        {
            var item = await _orderItemAppService.CreateOrderItemAsync(input);
            return CreatedAtAction(nameof(GetOrderItemsListAsync), new { orderId = item.OrderId }, item);
        }


        [HttpPut("{id}")]
        public async Task<OrderItemDto> UpdateOrderItemAsync(int id, [FromBody] UpdateOrderItemDto input)
        {
            return await _orderItemAppService.UpdateOrderItemAsync(id, input);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrderItemAsync(int id)
        {
            return await _orderItemAppService.DeleteOrderItemAsync(id);
        }
    }
}
