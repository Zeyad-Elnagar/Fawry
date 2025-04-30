using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Fawry.Data;

/* This is used if database provider does't define
 * IFawryDbSchemaMigrator implementation.
 */
public class NullFawryDbSchemaMigrator : IFawryDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
