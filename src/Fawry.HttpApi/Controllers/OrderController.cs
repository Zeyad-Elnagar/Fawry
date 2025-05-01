using Fawry.Orders;
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
    [Route("api/app/orders")]
    [ApiController]
    public class OrderController : AbpController
    {
        private readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto input)
        {
            var order = await _orderAppService.CreateOrderAsync(input);
            return CreatedAtAction(nameof(GetOrderAsync), new { id = order.Id }, order);
        }


        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderAppService.DeleteOrderAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<OrderDto> GetOrderAsync(int id)
        {
            return await _orderAppService.GetOrderAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<OrderDto> UpdateOrderAsync(int id, [FromBody] UpdateOrderDto input)
        {
            return await _orderAppService.UpdateOrderAsync(id, input);
        }

        [HttpGet("list")]
        public async Task<PagedResultDto<OrderDto>> GetOrderListAsync([FromQuery] GetOrderListDto input)
        {
            return await _orderAppService.GetListAsync(input);
        }

        //[HttpPut("{id}/status")]
        //public async Task<OrderDto> ChangeOrderStatusAsync(int id, [FromQuery] OrderStatu newStatus)
        //{
        //    return await _orderAppService.ChangeOrderStatusAsync(id, newStatus);
        //}

        //[HttpPut("{id}/confirm-payment")]
        //public async Task<OrderDto> ConfirmOrderAsync(int id)
        //{
        //    return await _orderAppService.ConfirmOrderAsync(id);
        //}

        [HttpPut("{id}/confirm-payment")]
        public async Task<OrderDto> ConfirmOrderAsync(int id, [FromBody] ConfirmOrderDto input)
        {
            return await _orderAppService.ConfirmOrderAsync(id, input);
        }



    }
}
