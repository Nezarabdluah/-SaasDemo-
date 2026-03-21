using Volo.Abp.Threading;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace SaasDemo;

public static class SaasDemoGlobalFeatureConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
            {
                cmsKit.Blogging.Enable();
                cmsKit.Comments.Enable();
                cmsKit.Tags.Enable();
                cmsKit.Ratings.Enable();
                cmsKit.Reactions.Enable();
                cmsKit.Pages.Enable();
            });
        });
    }
}
