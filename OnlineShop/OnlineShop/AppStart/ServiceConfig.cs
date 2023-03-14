using OnlineShop.Service.Services.FileExcute;
using OnlineShop.Service.Services;
using OnlineShop.Service.Services.Token;
using OnlineShop.Service.Services.UserService;
using OnlineShop.Data.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using OnlineShop.Data.Repositories;

namespace OnlineShop.AppStart
{
    public class ServiceConfig
    {
        public static void RegisterServices(ref WebApplicationBuilder builder)
        {
            var corsUrls = builder.Configuration.GetSection("Cors:WithOrigins").Get<string[]>();
            var corsMethods = builder.Configuration.GetSection("Cors:WithMethods").Get<string[]>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
            policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                policy.WithOrigins(corsUrls)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithMethods(corsMethods);
            }));

            // File Excute
            builder.Services.AddTransient<IFileUpload, FileUpload>();
            builder.Services.AddTransient<IFileDownload, FileDownload>();
            builder.Services.AddTransient<IFileDelete, FileDelete>();

            // String compress
            builder.Services.AddTransient<IStringCompression, StringCompression>();

            // Token
            builder.Services.AddScoped<ITokenManager, TokenManager>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Add db context factory
            builder.Services.AddScoped<IDbFactory, DbFactory>();
            // Add repository
            builder.Services.AddScoped<IApplicationGroupRepository, ApplicationGroupRepository>();
            builder.Services.AddScoped<IApplicationRoleGroupRepository, ApplicationRoleGroupRepository>();
            builder.Services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();
            builder.Services.AddScoped<IApplicationUserGroupRepository, ApplicationUserGroupRepository>();

            builder.Services.AddScoped<IAboutRepository, AboutRepository>();
            builder.Services.AddScoped<IContactDetailRepository, ContactDetailRepository>();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IErrorRepository, ErrorRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            builder.Services.AddScoped<IFooterRepository, FooterRepository>();
            builder.Services.AddScoped<IMenuGroupRepository, MenuGroupRepository>();
            builder.Services.AddScoped<IMenuRepository, MenuRepository>();
            builder.Services.AddScoped<IMenuTypeRepository, MenuTypeRepository>();
            builder.Services.AddScoped<ISlideRepository, SlideRepository>();

            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();

            builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();

            builder.Services.AddScoped<ISupportOnlineRepository, SupportOnlineRepository>();
            builder.Services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IVisitorStatisticRepository, VisitorStatisticRepository>();

            // Add unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddRazorPages();

            // add mail sender service
            builder.Services.Configure<SenderSettings>(builder.Configuration.GetSection("MailSender"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
