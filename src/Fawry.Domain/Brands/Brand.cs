using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Fawry.Brands
{
    public class Brand : FullAuditedEntity<int>
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
