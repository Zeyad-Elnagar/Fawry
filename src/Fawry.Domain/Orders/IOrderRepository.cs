using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Fawry.Orders
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        Task<List<Order>> GetListAsync(int skipCount, int maxResultCount);
        Task<int> CountAsync();
    }
}
