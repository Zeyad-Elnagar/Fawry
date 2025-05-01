using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Fawry.CustomersAddress
{
    public interface ICustomerAddressAppService : IApplicationService
    {
        Task<List<CustomerAddressDto>> GetAddressesAsync(); // based on current user
        Task<CustomerAddressDto> GetAddressAsync(int id);
        Task<CustomerAddressDto> CreateAddressAsync(CreateCustomerAddressDto input);
        Task<CustomerAddressDto> UpdateAddressAsync(UpdateCustomerAddressDto input);
        Task DeleteAddressAsync(int id);
    }
}
