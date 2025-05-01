using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawry.Products
{
    public class CreateProductDto
    {
        [FromForm]
        public string Name { get; set; }
        [FromForm]
        public string Description { get; set; }
        [FromForm]
        public decimal Price { get; set; }
        [FromForm]
        public int StockQuantity { get; set; }
        [FromForm]
        public int CategoryId { get; set; }
        [FromForm]
        public int BrandId { get; set; }

    }
}
