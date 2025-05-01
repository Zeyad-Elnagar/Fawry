using AutoMapper.Internal.Mappers;
using Fawry.Bases;
using Fawry.Brands;
using Fawry.Categories;
using Fawry.Products.ProductImages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Fawry.Products
{
    [RemoteService(false)]
    public class ProductAppService : BaseApplicationService, IProductAppService
    {
        #region filds
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<ProductImage, int> _productImageRepository;
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<Brand, int> _brandRepository;
        #endregion

        #region ctor
        public ProductAppService(
        IRepository<Product, int> productRepository,
        IRepository<ProductImage, int> productImageRepository,
        IRepository<Brand, int> brandRepository,
        IRepository<Category, int> categoryRepository)

        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }
        #endregion

        #region ProductAppService

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return false;
            }
            await _productRepository.DeleteAsync(product, autoSave: true);
            return true;
        }

        public async Task<PagedResultDto<ProductDto>> GetListAsync(GetProductListDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Product.Id);
            }
            var productsQuery = await _productRepository
                .WithDetailsAsync(Product => Product.Category,
                                  Product => Product.Brand);

            var products = await productsQuery
                .AsQueryable()
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    Product => Product.Name.Contains(input.Filter)
                )
                .WhereIf(
                    input.CategoryId.HasValue,
                    product => product.CategoryId == input.CategoryId
                )
                .WhereIf(
                    input.BrandId.HasValue,
                    product => product.BrandId == input.BrandId
                )
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(product => EF.Property<object>(product, input.Sorting))
                .ToListAsync();

            var totalCount = input.Filter == null
                ? await _productRepository.CountAsync()
                : await _productRepository.CountAsync(Product =>
                    Product.Name.Contains(input.Filter));

            return new PagedResultDto<ProductDto>(
                totalCount,
                ObjectMapper.Map<List<Product>, List<ProductDto>>(products)
            );
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var product = await _productRepository
                .WithDetailsAsync(Product => Product.Category,
                                  Product => Product.Brand)
                .Result
                .FirstOrDefaultAsync(Product => Product.Id == id);
            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), id);
            }
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto input)
        {
            // التحقق من وجود CategoryId
            var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == input.CategoryId);
            if (category == null)
            {
                throw new UserFriendlyException($"الفئة بمعرف {input.CategoryId} غير موجودة.");
            }

            // التحقق من وجود BrandId
            var brand = await _brandRepository.FirstOrDefaultAsync(b => b.Id == input.BrandId);
            if (brand == null)
            {
                throw new UserFriendlyException($"العلامة التجارية بمعرف {input.BrandId} غير موجودة.");
            }

            // تحويل الـ DTO إلى كيان المنتج
            var product = ObjectMapper.Map<CreateProductDto, Product>(input);
            await _productRepository.InsertAsync(product, autoSave: true);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }


        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto input)
        {
            var product = await _productRepository.GetAsync(id)
                          ?? throw new EntityNotFoundException(typeof(Product), id);
            // التحقق من وجود الفئة (Category)
            var category = await _categoryRepository.FirstOrDefaultAsync(c => c.Id == input.CategoryId);
            if (category == null)
            {
                throw new UserFriendlyException($"الفئة بمعرف {input.CategoryId} غير موجودة.");
            }

            // التحقق من وجود العلامة التجارية (Brand)
            var brand = await _brandRepository.FirstOrDefaultAsync(b => b.Id == input.BrandId);
            if (brand == null)
            {
                throw new UserFriendlyException($"العلامة التجارية بمعرف {input.BrandId} غير موجودة.");
            }
            ObjectMapper.Map(input, product);
            await _productRepository.UpdateAsync(product);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        #endregion

        #region ProductImageAppService
        public async Task<List<ProductImageDto>> AddImageAsync(int productId, IEnumerable<IRemoteStreamContent> imageFiles)
        {
            // تحقق من وجود المنتج
            var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                throw new UserFriendlyException("المنتج غير موجود.");

            var existingCount = await _productImageRepository.CountAsync(i => i.ProductId == productId);
            if (existingCount + imageFiles.Count() > 10)
                throw new UserFriendlyException("لا يمكن إضافة أكثر من 10 صور للمنتج.");

            var savedImages = new List<ProductImageDto>();

            foreach (var file in imageFiles)
            {
                if (file == null || file.GetStream().Length == 0)
                    throw new UserFriendlyException("لا يمكن رفع ملف فارغ.");

                var extension = Path.GetExtension(file.FileName)?.ToLower();
                if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                    throw new UserFriendlyException("الملفات المدعومة فقط JPG و PNG.");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine("wwwroot", "images", "products");
                var filePath = Path.Combine(folderPath, fileName);

                Directory.CreateDirectory(folderPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.GetStream().CopyToAsync(stream);
                }

                var image = new ProductImage
                {
                    ProductId = productId,
                    ImagePath = $"/images/products/{fileName}"
                };

                await _productImageRepository.InsertAsync(image);
                savedImages.Add(ObjectMapper.Map<ProductImage, ProductImageDto>(image));
            }

            return savedImages;
        }



        public async Task<List<ProductImageDto>> GetImagesAsync(int productId)
        {
            var images = await _productImageRepository.GetListAsync(i => i.ProductId == productId);
            return ObjectMapper.Map<List<ProductImage>, List<ProductImageDto>>(images);
        }

        public async Task<ProductImageDto> UpdateImageAsync(int productId, int imageId, IRemoteStreamContent newImageFile)
        {
            var product = await _productRepository.FirstOrDefaultAsync(c => c.Id == productId);
            if (product == null)
            {
                throw new UserFriendlyException($"الفئة بمعرف {productId} غير موجودة.");
            }

            var image = await _productImageRepository.GetAsync(imageId);

            if (image.ProductId != productId)
                throw new UserFriendlyException("This image does not belong to this product.");

            // حذف الصورة القديمة من السيرفر لو موجودة
            var oldImagePath = Path.Combine("wwwroot", image.ImagePath.TrimStart('/'));
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }

            // رفع الصورة الجديدة
            var extension = Path.GetExtension(newImageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine("wwwroot/images/products", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await newImageFile.GetStream().CopyToAsync(stream);
            }

            // تحديث مسار الصورة
            image.ImagePath = $"/images/products/{fileName}";
            await _productImageRepository.UpdateAsync(image);

            return ObjectMapper.Map<ProductImage, ProductImageDto>(image);
        }

        public async Task<bool> DeleteImageAsync(int productId, int imageId)
        {
            var image = await _productImageRepository.GetAsync(imageId);
            if (image.ProductId != productId)
                throw new UserFriendlyException("This image does not belong to this product.");
            await _productImageRepository.DeleteAsync(image);
            return true;
        }
        #endregion
    }
}
