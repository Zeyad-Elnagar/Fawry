using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Fawry.Orders.OrderStatus;
namespace Fawry.Orders
{
    public class GetOrderListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public OrderStatu? Status { get; set; }
        public int? CustomerId { get; set; }
    }
}
