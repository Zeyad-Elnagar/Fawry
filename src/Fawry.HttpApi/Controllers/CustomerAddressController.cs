using Fawry.CustomersAddress;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.Controllers
{
    [Route("api/customer-addresses")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressAppService _customerAddressAppService;

        public CustomerAddressController(ICustomerAddressAppService customerAddressAppService)
        {
            _customerAddressAppService = customerAddressAppService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerAddressDto>>> GetAllAsync()
        {
            var addresses = await _customerAddressAppService.GetAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddressDto>> GetByIdAsync(int id)
        {
            var address = await _customerAddressAppService.GetAddressAsync(id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerAddressDto>> CreateAsync([FromBody] CreateCustomerAddressDto input)
        {
            var address = await _customerAddressAppService.CreateAddressAsync(input);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = address.Id }, address);
        }

        [HttpPut]
        public async Task<ActionResult<CustomerAddressDto>> UpdateAsync([FromBody] UpdateCustomerAddressDto input)
        {
            var address = await _customerAddressAppService.UpdateAddressAsync(input);
            return Ok(address);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _customerAddressAppService.DeleteAddressAsync(id);
            return NoContent();
        }
    }
}
