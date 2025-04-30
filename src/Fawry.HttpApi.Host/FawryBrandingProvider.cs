using Microsoft.Extensions.Localization;
using Fawry.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Fawry;

[Dependency(ReplaceServices = true)]
public class FawryBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<FawryResource> _localizer;

    public FawryBrandingProvider(IStringLocalizer<FawryResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
