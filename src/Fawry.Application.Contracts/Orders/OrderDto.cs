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
    public class OrderDto : FullAuditedEntityDto<int>
    {
        public Guid CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public int? CustomerAddressId { get; set; }
        public string CustomerAddressText { get; set; }

        public DateTime OrderDate { get; set; }
        public OrderStatu Status { get; set; }
        public string StatusText => Status.ToString();

        public decimal TotalAmount { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
