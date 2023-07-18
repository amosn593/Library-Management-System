using AspNetCoreHero.ToastNotification;
using DAL.Data;
using DAL.Repositories;
using DOMAIN.IConfiguration;
using DOMAIN.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using USERVIEW.Areas.Identity.Data;
using REPORTS.Helpers;
using Serilog;
using USERVIEW.StartApp;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Library");


builder.Services.AddDbContext<USERVIEWContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDbContext<LibraryDbContext>(
    options => options.UseSqlServer(connectionString)
    );

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

//adding serilog logger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//Adding Repository Services to DI
builder.Services.RepositoryService();


//Adding Authentication Service
builder.Services.AuthenticationService();


//Adding Notification service
builder.Services.NotificationService();


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
