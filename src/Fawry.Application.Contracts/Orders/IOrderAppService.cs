using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Fawry.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> GetOrderAsync(int id);
        Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto input);
        Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto input);
        Task<bool> DeleteOrderAsync(int id);
        Task<OrderDto> ConfirmOrderAsync(int orderId, ConfirmOrderDto input);
    }
}
