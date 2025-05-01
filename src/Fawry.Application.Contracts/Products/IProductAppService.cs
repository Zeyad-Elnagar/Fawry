using Fawry.Products.ProductImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Fawry.Products
{
    public interface IProductAppService
    {
        Task<ProductDto> GetProductAsync(int id);
        Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListDto input);
        Task<ProductDto> CreateProductAsync(CreateProductDto input);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto input);
        Task<bool> DeleteProductAsync(int id);

        Task<List<ProductImageDto>> GetImagesAsync(int productId);
        Task<List<ProductImageDto>> AddImageAsync(int productId, IEnumerable<IRemoteStreamContent> imageFiles);
        Task<ProductImageDto> UpdateImageAsync(int productId, int imageId, IRemoteStreamContent newImageFile);
        Task<bool> DeleteImageAsync(int productId, int imageId);
    }
}
