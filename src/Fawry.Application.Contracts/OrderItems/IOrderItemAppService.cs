using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Fawry.OrderItems
{
    public interface IOrderitemAppService : IApplicationService
    {
        Task<PagedResultDto<OrderItemDto>> GetOrderItemsListAsync(int id);
        Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto input);
        Task<OrderItemDto> UpdateOrderItemAsync(int id, UpdateOrderItemDto input);
        Task<bool> DeleteOrderItemAsync(int id);
    }
}
