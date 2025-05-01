using Fawry.CustomersAddress;
using Fawry.OrderItems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.Orders
{
    public class CreateOrderDto
    {
        [FromForm]
        public int? CustomerAddressId { get; set; }

        [FromForm]
        public CreateCustomerAddressDto? NewAddress { get; set; }

        [FromForm]
        public int PaymentTypeId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
