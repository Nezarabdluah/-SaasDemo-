using Microsoft.Extensions.Localization;
using SaasDemo.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SaasDemo;

[Dependency(ReplaceServices = true)]
public class SaasDemoBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SaasDemoResource> _localizer;

    public SaasDemoBrandingProvider(IStringLocalizer<SaasDemoResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
