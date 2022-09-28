using AspNetCoreHero.ToastNotification;
using DAL.Data;
using DAL.Repositories;
using DOMAIN.IConfiguration;
using DOMAIN.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using USERVIEW.Data;
using USERVIEW.Areas.Identity.Data;
using REPORTS.Helpers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Library");



builder.Services.AddDbContext<USERVIEWContext>(options =>
    options.UseSqlServer(connectionString));




builder.Services.AddDbContext<LibraryDbContext>(
    options => options.UseSqlServer(connectionString)
    );

// Add services to the container.
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 4;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});




builder.Services.AddDefaultIdentity<USERVIEWUser>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<USERVIEWContext>();



builder.Services.AddAuthorization(options => {
    options.AddPolicy("Basic",
    policy => policy.RequireRole("Captain"));
    options.AddPolicy("Medium",
      policy => policy.RequireRole("Librarian"));
    options.AddPolicy("Premium",
      policy => policy.RequireRole("Principal"));
    options.AddPolicy("Admin",
      policy => policy.RequireRole("Admin"));
    
});



builder.Services.Configure<IdentityOptions>(options =>
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

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(25);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for
    // production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Borrowing}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
