using Volo.Abp.Settings;

namespace Fawry.Settings;

public class FawrySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(FawrySettings.MySetting1));
    }
}
