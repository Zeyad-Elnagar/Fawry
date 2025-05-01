using Fawry.PaymentTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Fawry.Controllers
{
    [Route("api/app/payment")]
    [ApiController]
    public class PaymentTypeController : AbpController
    {
        private readonly IPaymentTypeAppService _paymentTypeAppService;
        public PaymentTypeController(IPaymentTypeAppService paymentTypeAppService)
        {
            _paymentTypeAppService = paymentTypeAppService;
        }


        [HttpPost("create")]
        public async Task<PaymentTypeDto> CreatePaymentTypeAsync(CreatePaymentTypeDto input)
        {
            return await _paymentTypeAppService.CreatePaymentTypeAsync(input);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeletePaymentTypeAsync(int id)
        {
            await _paymentTypeAppService.DeletePaymentTypeAsync(id);
            return true;
        }

        [HttpGet("List")]
        public async Task<PagedResultDto<PaymentTypeDto>> GetListAsync()
        {
            return await _paymentTypeAppService.GetListAsync();
        }

        [HttpGet("{id}")]
        public async Task<PaymentTypeDto> GetPaymentTypeAsync(int id)
        {
            return await _paymentTypeAppService.GetPaymentTypeAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<PaymentTypeDto> UpdatePaymentTypeAsync(int id, UpdatePaymentTypeDto input)
        {
            return await _paymentTypeAppService.UpdatePaymentTypeAsync(id, input);
        }
    }
}
