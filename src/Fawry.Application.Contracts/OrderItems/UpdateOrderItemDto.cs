using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Fawry.OrderItems
{
    public class UpdateOrderItemDto : EntityDto<int>
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
