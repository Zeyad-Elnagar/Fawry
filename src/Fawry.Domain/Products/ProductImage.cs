using Fawry.Brands;
using Fawry.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Fawry.Products
{
    public class ProductImage : FullAuditedEntity<int>
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string ImagePath { get; set; }
    }
}
