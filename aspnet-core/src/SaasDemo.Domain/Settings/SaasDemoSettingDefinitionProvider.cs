using Volo.Abp.Settings;

namespace SaasDemo.Settings;

public class SaasDemoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SaasDemoSettings.MySetting1));
    }
}
