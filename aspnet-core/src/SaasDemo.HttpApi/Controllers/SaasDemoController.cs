using SaasDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SaasDemo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SaasDemoController : AbpControllerBase
{
    protected SaasDemoController()
    {
        LocalizationResource = typeof(SaasDemoResource);
    }
}
