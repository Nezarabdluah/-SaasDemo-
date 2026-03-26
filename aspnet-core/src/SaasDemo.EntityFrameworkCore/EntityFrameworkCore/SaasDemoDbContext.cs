using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using SaasDemo.BlogPosts;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace SaasDemo.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class SaasDemoDbContext :
    AbpDbContext<SaasDemoDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<BlogTag> BlogTags { get; set; }
    public DbSet<BlogPostTag> BlogPostTags { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<BlogPostCategory> BlogPostCategories { get; set; }
    public DbSet<SlugRedirect> SlugRedirects { get; set; }
    public DbSet<BlogPostVersion> BlogPostVersions { get; set; }



    public SaasDemoDbContext(DbContextOptions<SaasDemoDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.ConfigureBlogging();
        builder.ConfigureCmsKit();

        builder.Entity<BlogPost>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "BlogPosts", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => x.Slug).IsUnique();
            b.Property(x => x.MetaTitle).HasMaxLength(70);
            b.Property(x => x.MetaDescription).HasMaxLength(160);
        });

        builder.Entity<BlogCategory>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "BlogCategories", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<BlogPostCategory>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "BlogPostCategories", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
            
            // Define Composite Key for Many-to-Many strict relation
            b.HasKey(x => new { x.BlogPostId, x.BlogCategoryId });
            
            b.HasOne<BlogPost>().WithMany().HasForeignKey(x => x.BlogPostId).IsRequired();
            b.HasOne<BlogCategory>().WithMany().HasForeignKey(x => x.BlogCategoryId).IsRequired();
            
            b.HasIndex(x => new { x.BlogPostId, x.BlogCategoryId });
        });

        builder.Entity<BlogTag>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "BlogTags", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<BlogPostTag>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "BlogPostTags", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.HasKey(x => new { x.BlogPostId, x.BlogTagId });
            b.HasOne<BlogPost>().WithMany().HasForeignKey(x => x.BlogPostId).IsRequired();
            b.HasOne<BlogTag>().WithMany().HasForeignKey(x => x.BlogTagId).IsRequired();
            b.HasIndex(x => new { x.BlogPostId, x.BlogTagId });
        });

        builder.Entity<SlugRedirect>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "SlugRedirects", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => x.OldSlug).IsUnique();
            b.HasOne<BlogPost>().WithMany().HasForeignKey(x => x.BlogPostId).IsRequired();
        });

        builder.Entity<BlogPostVersion>(b =>
        {
            b.ToTable(SaasDemoConsts.DbTablePrefix + "BlogPostVersions", SaasDemoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => new { x.BlogPostId, x.VersionNumber }).IsUnique();
            b.HasOne<BlogPost>().WithMany().HasForeignKey(x => x.BlogPostId).IsRequired();
            b.Property(x => x.MetaTitle).HasMaxLength(70);
            b.Property(x => x.MetaDescription).HasMaxLength(160);
        });

    }
}
