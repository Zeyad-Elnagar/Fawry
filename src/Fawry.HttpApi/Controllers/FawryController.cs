using Fawry.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Fawry.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class FawryController : AbpControllerBase
{
    protected FawryController()
    {
        LocalizationResource = typeof(FawryResource);
    }
}
