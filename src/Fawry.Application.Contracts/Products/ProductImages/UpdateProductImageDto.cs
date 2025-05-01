using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Fawry.Products.ProductImages
{
    public class UpdateProductImageDto : EntityDto<int>
    {
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
    }
}
