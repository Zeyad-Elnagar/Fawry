using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Fawry.PaymentTypes
{
    public class UpdatePaymentTypeDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
