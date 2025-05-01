using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.Orders
{
    public class ConfirmOrderDto
    {
        public int PaymentTypeId { get; set; }
        public int CustomerAddressId { get; set; }
    }

}
