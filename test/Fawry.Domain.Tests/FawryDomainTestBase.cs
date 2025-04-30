using Volo.Abp.Modularity;

namespace Fawry;

/* Inherit from this class for your domain layer tests. */
public abstract class FawryDomainTestBase<TStartupModule> : FawryTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
