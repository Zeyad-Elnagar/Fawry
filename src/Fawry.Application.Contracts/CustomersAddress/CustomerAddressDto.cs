using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Fawry.CustomersAddress
{
    public class CustomerAddressDto : EntityDto<int>
    {
        public string Title { get; set; }             // مثل "البيت" أو "الشغل"
        public string AddressLine { get; set; }       // العنوان الكامل
        public bool IsDefault { get; set; }
    }
}
