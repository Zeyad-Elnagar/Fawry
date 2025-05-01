using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Fawry.CustomersAddress
{
    public class UpdateCustomerAddressDto : EntityDto<int>
    {
        public string Title { get; set; }
        public string AddressLine { get; set; }
        public bool IsDefault { get; set; }
    }
}
