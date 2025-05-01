using AutoMapper.Internal.Mappers;
using Fawry.Bases;
using Fawry.CustomersAddress;
using Fawry.OrderItems;
using Fawry.Orders.OrderStatus;
using Fawry.Orders;
using Fawry.PaymentTypes;
using Fawry.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp;

namespace Fawry.CustomerAddresses
{
    [RemoteService(false)]
    public class CustomerAddressAppService : BaseApplicationService, ICustomerAddressAppService
    {
        private readonly IRepository<CustomerAddress, int> _addressRepository;
        private readonly ICurrentUser _currentUser;

        public CustomerAddressAppService(
            IRepository<CustomerAddress, int> addressRepository,
            ICurrentUser currentUser)
        {
            _addressRepository = addressRepository;
            _currentUser = currentUser;
        }

        private Guid GetCurrentUserId()
        {
            if (!_currentUser.Id.HasValue)
                throw new UserFriendlyException("You must be logged in.");
            return _currentUser.Id.Value;
        }

        public async Task<List<CustomerAddressDto>> GetAddressesAsync()
        {
            var userId = GetCurrentUserId();
            var addresses = await _addressRepository.GetListAsync(x => x.UserId == userId);
            return ObjectMapper.Map<List<CustomerAddress>, List<CustomerAddressDto>>(addresses);
        }

        public async Task<CustomerAddressDto> GetAddressAsync(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            if (address.UserId != GetCurrentUserId())
                throw new UserFriendlyException("You don't have access to this address.");
            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(address);
        }

        public async Task<CustomerAddressDto> CreateAddressAsync(CreateCustomerAddressDto input)
        {
            var userId = GetCurrentUserId();

            if (input.IsDefault)
            {
                var existingDefaults = await _addressRepository.GetListAsync(x => x.UserId == userId && x.IsDefault);
                foreach (var a in existingDefaults)
                {
                    a.IsDefault = false;
                    await _addressRepository.UpdateAsync(a);
                }
            }

            var address = new CustomerAddress
            {
                UserId = userId,
                Title = input.Title,
                AddressLine = input.AddressLine,
                IsDefault = input.IsDefault
            };

            await _addressRepository.InsertAsync(address);
            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(address);
        }

        public async Task<CustomerAddressDto> UpdateAddressAsync(UpdateCustomerAddressDto input)
        {
            var address = await _addressRepository.GetAsync(input.Id);
            if (address.UserId != GetCurrentUserId())
                throw new UserFriendlyException("You can't update this address.");

            if (input.IsDefault)
            {
                var userId = GetCurrentUserId();
                var existingDefaults = await _addressRepository.GetListAsync(x => x.UserId == userId && x.IsDefault);
                foreach (var a in existingDefaults.Where(a => a.Id != input.Id))
                {
                    a.IsDefault = false;
                    await _addressRepository.UpdateAsync(a);
                }
            }

            address.Title = input.Title;
            address.AddressLine = input.AddressLine;
            address.IsDefault = input.IsDefault;

            await _addressRepository.UpdateAsync(address);
            return ObjectMapper.Map<CustomerAddress, CustomerAddressDto>(address);
        }

        public async Task DeleteAddressAsync(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            if (address.UserId != GetCurrentUserId())
                throw new UserFriendlyException("You can't delete this address.");

            await _addressRepository.DeleteAsync(id);
        }
    }
}
