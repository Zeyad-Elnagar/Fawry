using Fawry.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Fawry.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(FawryEntityFrameworkCoreModule),
    typeof(FawryApplicationContractsModule)
)]
public class FawryDbMigratorModule : AbpModule
{
}
