using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Model.Models;

namespace OnlineShop.Data;

public class ShopOnlineDbContext : IdentityDbContext<ApplicationUser>
{
    public ShopOnlineDbContext(DbContextOptions<ShopOnlineDbContext> options)
        : base(options)
    {

    }

    public DbSet<About> Abouts { get; set; } = default!;
    public DbSet<Contact> Contacts { get; set; } = default!;
    public DbSet<ContactDetail> ContactDetails { get; set; } = default!;
    public DbSet<Error> Errors { get; set; } = default!;
    public DbSet<Feedback> Feedbacks { get; set; } = default!;
    public DbSet<Footer> Footers { get; set; } = default!;
    public DbSet<Menu> Menus { get; set; } = default!;
    public DbSet<MenuGroup> MenuGroups { get; set; } = default!;
    public DbSet<MenuType> MenuTypes { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
    public DbSet<PostCategory> PostCategories { get; set; } = default!;
    public DbSet<PostContent> PostContents { get; set; } = default!;
    public DbSet<PostTag> PostTags { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = default!;
    public DbSet<ProductTags> ProductTags { get; set; } = default!;
    public DbSet<Slide> Slides { get; set; } = default!;
    public DbSet<SupportOnline> SupportOnlines { get; set; } = default!;
    public DbSet<SystemConfig> SystemConfigs { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<VisitorStatistic> VisitorStatistics { get; set; } = default!;

    public DbSet<ApplicationGroup> ApplicationGroups { set; get; } = default!;
    public DbSet<ApplicationRole> ApplicationRoles { set; get; } = default!;
    public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; } = default!;
    public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Delete "AspNet" in table name
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName!.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Replace("AspNet", "Application"));
                //if(tableName.Contains("User") || tableName.Contains("Role"))
                //{
                //    var newTableName = tableName.Substring(6);
                //    newTableName = "Application" + newTableName;
                //    entityType.SetTableName(newTableName);
                //}
                //else
                //{
                //    entityType.SetTableName(tableName.Substring(6));
                //}         
            }
        }

        builder.Entity<ApplicationRoleGroup>().HasKey(u => new
        {
            u.RoleId,
            u.GroupId
        });
        builder.Entity<ApplicationUserGroup>().HasKey(u => new
        {
            u.UserId,
            u.GroupId
        });

        builder.Entity<PostTag>().HasKey(u => new
        {
            u.TagID,
            u.ContentID
        });
        builder.Entity<ProductTags>().HasKey(u => new
        {
            u.TagID,
            u.ProductID
        });
        builder.Entity<OrderDetail>().HasKey(u => new
        {
            u.OrderID,
            u.ProductID
        });

        //builder.Entity<BaseUser>(entity =>
        //{
        //    //tell database to use this column as Discriminator
        //    entity.HasDiscriminator<string>("UserType");
        //});

        //// change col name in db
        //builder.Entity<BasicUser>().Property(p => p.Id).HasColumnName("user_id");
        //builder.Entity<BasicUser>().Property(p => p.UserName).HasColumnName("name");
        ////builder.Entity<BasicUser>().Property(p => p.GroupId).HasColumnName("group_id");
        //builder.Entity<BasicUser>().Property(p => p.ElectronicSeal).HasColumnName("electronic_seal");

        //// set unique with service name
        //builder.Entity<ServiceInfo>().HasIndex(p => p.Name).IsUnique();
    }
}
