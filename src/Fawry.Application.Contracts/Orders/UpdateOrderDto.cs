using Fawry.OrderItems;
using Fawry.Orders.OrderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Fawry.Orders
{
    public class UpdateOrderDto : EntityDto<int>
    {
        public int? CustomerAddressId { get; set; }
        public int PaymentTypeId { get; set; }
        public OrderStatu Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
