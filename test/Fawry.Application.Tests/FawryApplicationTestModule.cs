using Volo.Abp.Modularity;

namespace Fawry;

[DependsOn(
    typeof(FawryApplicationModule),
    typeof(FawryDomainTestModule)
)]
public class FawryApplicationTestModule : AbpModule
{

}
