using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace Fawry.Products.ProductImages
{
    public class CreateProductImageDto
    {
        [FromForm]
        public int ProductId { get; set; }
        [FromForm]
        public List<IRemoteStreamContent> Images { get; set; }
    }
}
