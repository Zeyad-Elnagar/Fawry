using Fawry.PaymentTypes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fawry.Mappings
{
    public class PaymentTypeMappingProfile : Profile
    {
        public PaymentTypeMappingProfile()
        {
            CreateMap<PaymentType, PaymentTypeDto>();
            CreateMap<CreatePaymentTypeDto, PaymentType>();
            CreateMap<UpdatePaymentTypeDto, PaymentType>();
        }
    }
}
