using AutoMapper;
using Fawry .Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            //.ForMember(x => x.Name, opt => opt.MapFrom(x => x.NameAr))  {{ ده لو في حالة ان الاسم هنا مش زي هنا}}
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
