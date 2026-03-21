namespace SaasDemo.Permissions;

public static class SaasDemoPermissions
{
    public const string GroupName = "SaasDemo";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public class BlogPost
    {
        public const string Default = GroupName + ".BlogPost";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
    /// <summary>
    /// Domain entity representing a category specifically for Blog Posts.
    /// Isolated from E-commerce or general categories to maintain Bounded Contexts.
    /// </summary>
    public class BlogCategory
    {
        public const string Default = GroupName + ".BlogCategory";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
