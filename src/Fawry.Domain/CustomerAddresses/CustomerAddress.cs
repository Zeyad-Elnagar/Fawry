using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace Fawry.CustomerAddresses
{
    public class CustomerAddress : Entity<int>
    {
        public Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        public string Title { get; set; }
        public string AddressLine { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
