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
        public int? CustomerAddressId { get; set; }
        public CreateCustomerAddressDto? NewAddress { get; set; }
        public int? PaymentTypeId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }
}
