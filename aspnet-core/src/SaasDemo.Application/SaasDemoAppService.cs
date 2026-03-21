using System;
using System.Collections.Generic;
using System.Text;
using SaasDemo.Localization;
using Volo.Abp.Application.Services;

namespace SaasDemo;

/* Inherit your application services from this class.
 */
public abstract class SaasDemoAppService : ApplicationService
{
    protected SaasDemoAppService()
    {
        LocalizationResource = typeof(SaasDemoResource);
    }
}
