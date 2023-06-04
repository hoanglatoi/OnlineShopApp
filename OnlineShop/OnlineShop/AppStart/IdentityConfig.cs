using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Model.Models;
using OnlineShop.Service.Services.Token;
using System.Text;

namespace OnlineShop.AppStart
{
    public class IdentityConfig
    {
        public static void RegisterIdentity(ref WebApplicationBuilder builder)
        {
            int tokenExpire = int.Parse(builder.Configuration.GetSection("TokenSettings:Expire").Value);
            var privateKey = builder.Configuration.GetSection("Jwt:JwtSecurityKey").Value;
            var CookieName = builder.Configuration.GetSection("CookieOptions:CookieName").Value;

            // Identity setting
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<OnlineShop.Data.ShopOnlineDbContext>()
            .AddDefaultTokenProviders();

            // IdentityCookie settings
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    options.Cookie.Name = "Identity-Access-Token";
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(tokenExpire);
            //    options.LoginPath = "/Identity/Account/Login";
            //    // ReturnUrlParameter requires 
            //    //using Microsoft.AspNetCore.Authentication.Cookies;
            //    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            //    options.SlidingExpiration = true;
            //});

            // Authentication config: read token from Headers.Authorization or Headers["InternalAuth"] or Cookie 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = builder.Configuration.GetSection("Jwt:JwtIssuer").Value,
                    //ValidAudience = builder.Configuration.GetSection("Jwt:JwtAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                        if (string.IsNullOrEmpty(token))
                        {
                            token = context.Request.Headers["InternalAuth"].ToString().Replace("Bearer ", "");
                            context.Token = token;
                        }
                        if (string.IsNullOrEmpty(token))
                        {
                            if (context.Request.Cookies.ContainsKey(CookieName))
                            {
                                // use cookie
                                context.Token = context.Request.Cookies[CookieName];
                            }
                            else
                            {
                                //context.Response.StatusCode = 403; //ForbidResult
                                //context.Response.ContentType = "text/plain";
                                //context.Response.WriteAsync("Must login to use this api").Wait();
                                context.Response.Redirect("/Identity/Account/Error?title=Error&errormsg=Must-login-to-use-this-api", false);
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.StatusCode = 410; // Gone
                            context.Response.ContentType = "text/plain";
                            context.Response.WriteAsync(context.Exception.Message).Wait();
                        }
                        else
                        {
                            context.Response.StatusCode = 403; //ForbidResult
                            context.Response.ContentType = "text/plain";
                            context.Response.WriteAsync(context.Exception.Message).Wait();
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });

            // Authorization: use scheme bearer token for default
            builder.Services.AddAuthorization(options =>
            {
                //var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                //    JwtBearerDefaults.AuthenticationScheme,
                //    CookieAuthenticationDefaults.AuthenticationScheme);
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();
        }
    }
}
