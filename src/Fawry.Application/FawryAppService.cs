using Fawry.Localization;
using Volo.Abp.Application.Services;

namespace Fawry;

/* Inherit your application services from this class.
 */
public abstract class FawryAppService : ApplicationService
{
    protected FawryAppService()
    {
        LocalizationResource = typeof(FawryResource);
    }
}
