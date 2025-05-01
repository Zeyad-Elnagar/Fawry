using Fawry.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Fawry.EntityFrameworkCore.Orders
{
    public class OrderRepository : EfCoreRepository<FawryDbContext, Order, int>, IOrderRepository
    {
        public OrderRepository(IDbContextProvider<FawryDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Order>> GetListAsync(int skipCount, int maxResultCount)
        {
            return await DbSet.Skip(skipCount).Take(maxResultCount).ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }
    }
}
