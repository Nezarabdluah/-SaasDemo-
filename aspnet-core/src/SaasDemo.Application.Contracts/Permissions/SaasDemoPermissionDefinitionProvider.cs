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

        var blogPostPermission = myGroup.AddPermission(SaasDemoPermissions.BlogPost.Default, L("Permission:BlogPost"));
        blogPostPermission.AddChild(SaasDemoPermissions.BlogPost.Create, L("Permission:Create"));
        blogPostPermission.AddChild(SaasDemoPermissions.BlogPost.Update, L("Permission:Update"));
        blogPostPermission.AddChild(SaasDemoPermissions.BlogPost.Delete, L("Permission:Delete"));

        var blogCategoryPermission = myGroup.AddPermission(SaasDemoPermissions.BlogCategory.Default, L("Permission:BlogCategory"));
        blogCategoryPermission.AddChild(SaasDemoPermissions.BlogCategory.Create, L("Permission:Create"));
        blogCategoryPermission.AddChild(SaasDemoPermissions.BlogCategory.Update, L("Permission:Update"));
        blogCategoryPermission.AddChild(SaasDemoPermissions.BlogCategory.Delete, L("Permission:Delete"));

        var blogTagPermission = myGroup.AddPermission(SaasDemoPermissions.BlogTag.Default, L("Permission:BlogTag"));
        blogTagPermission.AddChild(SaasDemoPermissions.BlogTag.Create, L("Permission:Create"));
        blogTagPermission.AddChild(SaasDemoPermissions.BlogTag.Update, L("Permission:Update"));
        blogTagPermission.AddChild(SaasDemoPermissions.BlogTag.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SaasDemoResource>(name);
    }
}
