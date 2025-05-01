using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Fawry.Brands
{
    public interface IBrandAppService : ICrudAppService<BrandDto, int, PagedAndSortedResultRequestDto, CreateBrandDto, UpdateBrandDto>
    {
    }
}
