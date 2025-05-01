using Fawry.Products.ProductImages;
using Fawry.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Fawry.Controllers
{
    [Route("api/app/product")]
    [ApiController]
    public class ProductController : AbpController
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpPost("create")]
        public async Task<ProductDto> CreateProductAsync(CreateProductDto input)
        {
            return await _productAppService.CreateProductAsync(input);
        }

        [HttpPut("update/{id}")]
        public async Task<ProductDto> UpdateProductAsync(int id, [FromBody] UpdateProductDto input)
        {
            return await _productAppService.UpdateProductAsync(id, input);
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> GetProductAsync(int id)
        {
            return await _productAppService.GetProductAsync(id);
        }

        [HttpGet("list")]
        public async Task<PagedResultDto<ProductDto>> GetListAsync([FromQuery] GetProductListDto input)
        {
            return await _productAppService.GetListAsync(input);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productAppService.DeleteProductAsync(id);
        }

        // Images
        [HttpGet("{productId}/images")]
        public async Task<List<ProductImageDto>> GetImagesAsync(int productId)
        {
            return await _productAppService.GetImagesAsync(productId);
        }

        [HttpPost("{productId}/images")]
        [Consumes("multipart/form-data")]
        public async Task<List<ProductImageDto>> AddImageAsync(int productId, [FromForm] List<IRemoteStreamContent> imageFiles)
        {
            return await _productAppService.AddImageAsync(productId, imageFiles);
        }



        [HttpPut("{productId}/images/{imageId}")]
        [ProducesResponseType(typeof(ProductImageDto), StatusCodes.Status200OK)]
        public async Task<ProductImageDto> UpdateImageAsync(int productId, int imageId, [FromForm(Name = "newImageFile")] IRemoteStreamContent newImageFile)
        {
            return await _productAppService.UpdateImageAsync(productId, imageId, newImageFile);
        }


        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<bool> DeleteImageAsync(int productId, int imageId)
        {
            return await _productAppService.DeleteImageAsync(productId, imageId);
        }

    }
}
