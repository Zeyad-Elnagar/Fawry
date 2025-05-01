using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.CustomersAddress
{
    public class CreateCustomerAddressDto
    {
        public string Title { get; set; }
        public string AddressLine { get; set; }
        public bool IsDefault { get; set; }
    }
}
