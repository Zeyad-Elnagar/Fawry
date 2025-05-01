using AutoMapper.Internal.Mappers;
using Fawry.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;

namespace Fawry.PaymentTypes
{
    [RemoteService(false)]
    public class PaymentTypeAppService : BaseApplicationService, IPaymentTypeAppService
    {
        private readonly IRepository<PaymentType, int> _paymentTypeRepository;
        public PaymentTypeAppService(IRepository<PaymentType, int> paymentTypeRepository)
        {
            _paymentTypeRepository = paymentTypeRepository;
        }

        public async Task<PaymentTypeDto> CreatePaymentTypeAsync(CreatePaymentTypeDto input)
        {
            var paymentType = ObjectMapper.Map<CreatePaymentTypeDto, PaymentType>(input);
            await _paymentTypeRepository.InsertAsync(paymentType, autoSave: true);
            return ObjectMapper.Map<PaymentType, PaymentTypeDto>(paymentType);
        }

        public async Task<bool> DeletePaymentTypeAsync(int id)
        {
            var paymentType = await _paymentTypeRepository.FindAsync(id);
            if (paymentType == null)
            {
                throw new EntityNotFoundException(typeof(PaymentType), id);
            }

            await _paymentTypeRepository.DeleteAsync(paymentType);
            return true;
        }

        public async Task<PagedResultDto<PaymentTypeDto>> GetListAsync()
        {
            var list = await _paymentTypeRepository.GetListAsync();
            var totalCount = list.Count;

            var result = ObjectMapper.Map<List<PaymentType>, List<PaymentTypeDto>>(list);

            return new PagedResultDto<PaymentTypeDto>
            {
                TotalCount = totalCount,
                Items = result
            };
        }

        public async Task<PaymentTypeDto> GetPaymentTypeAsync(int id)
        {
            var paymentType = await _paymentTypeRepository.FindAsync(id);
            if (paymentType == null)
            {
                throw new EntityNotFoundException(typeof(PaymentType), id);
            }
            return ObjectMapper.Map<PaymentType, PaymentTypeDto>(paymentType);
        }

        public async Task<PaymentTypeDto> UpdatePaymentTypeAsync(int id, UpdatePaymentTypeDto input)
        {
            var paymentType = await _paymentTypeRepository.FindAsync(id);
            if (paymentType == null)
            {
                throw new EntityNotFoundException(typeof(PaymentType), id);
            }
            ObjectMapper.Map(input, paymentType);
            await _paymentTypeRepository.UpdateAsync(paymentType, autoSave: true);
            return ObjectMapper.Map<PaymentType, PaymentTypeDto>(paymentType);
        }
    }
}
