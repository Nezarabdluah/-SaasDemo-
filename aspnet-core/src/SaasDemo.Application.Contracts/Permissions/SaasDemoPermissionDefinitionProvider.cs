using SaasDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SaasDemo.Permissions;

public class SaasDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SaasDemoPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SaasDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SaasDemoResource>(name);
    }
}
