using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Fawry.PaymentTypes
{
    public interface IPaymentTypeAppService : IApplicationService
    {
        Task<PaymentTypeDto> GetPaymentTypeAsync(int id);
        Task<PagedResultDto<PaymentTypeDto>> GetListAsync();
        Task<PaymentTypeDto> CreatePaymentTypeAsync(CreatePaymentTypeDto input);
        Task<PaymentTypeDto> UpdatePaymentTypeAsync(int id, UpdatePaymentTypeDto input);
        Task<bool> DeletePaymentTypeAsync(int id);
    }
}
