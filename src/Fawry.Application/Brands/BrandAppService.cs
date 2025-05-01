using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Fawry.Brands
{
    public class BrandsAppService : CrudAppService<Brand, BrandDto, int, PagedAndSortedResultRequestDto, CreateBrandDto, UpdateBrandDto>,
       IBrandAppService
    {
        public BrandsAppService(IRepository<Brand, int> repository)
            : base(repository)
        {
        }
    }
}
