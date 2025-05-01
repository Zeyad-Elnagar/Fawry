using AutoMapper;
using Fawry.CustomersAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fawry.CustomerAddresses;

namespace Fawry.Mappings
{
    public class CustomerAddressMappingProfile : Profile
    {
        public CustomerAddressMappingProfile()
        {
            CreateMap<CustomerAddress, CustomerAddressDto>();
            CreateMap<CreateCustomerAddressDto, CustomerAddress>();
            CreateMap<UpdateCustomerAddressDto, CustomerAddress>();
                
        }
    }
}
