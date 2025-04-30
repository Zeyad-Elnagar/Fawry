using Volo.Abp.Modularity;

namespace Fawry;

public abstract class FawryApplicationTestBase<TStartupModule> : FawryTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
