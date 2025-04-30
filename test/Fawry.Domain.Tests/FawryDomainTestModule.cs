using Volo.Abp.Modularity;

namespace Fawry;

[DependsOn(
    typeof(FawryDomainModule),
    typeof(FawryTestBaseModule)
)]
public class FawryDomainTestModule : AbpModule
{

}
