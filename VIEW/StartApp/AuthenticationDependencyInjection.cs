using Microsoft.AspNetCore.Identity;
using USERVIEW.Areas.Identity.Data;
using USERVIEW.Data;

namespace USERVIEW.StartApp
{
    public static class AuthenticationDependencyInjection
    {
        public static IServiceCollection AuthenticationService(this IServiceCollection Services)
        {
            //Adding Default Identity
            Services.AddDefaultIdentity<USERVIEWUser>(
            options => options.SignIn.RequireConfirmedAccount = false
            )
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<USERVIEWContext>();


            //Adding Authorization
            Services.AddAuthorization(options => {
                options.AddPolicy("Basic",
                policy => policy.RequireRole("Captain"));
                options.AddPolicy("Medium",
                  policy => policy.RequireRole("Librarian"));
                options.AddPolicy("Premium",
                  policy => policy.RequireRole("Principal"));
                options.AddPolicy("Admin",
                  policy => policy.RequireRole("Admin"));

            });

            //Configure Identity Options
            Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#$&()!/";
                options.User.RequireUniqueEmail = true;
            });

            //Configure Application Cookies
            Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(25);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });


            return Services;
        }
    }
}
