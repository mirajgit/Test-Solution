using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using Test.Entities;
using Test.UI.Hubs;
using Test.UI.MiddlewareExtensions;
using Test.UI.Services.Interface;
using Test.UI.Services.Repository;
using Test.UI.SubscribeTableDependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure session settings
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".YourApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

// Configure DbContext
builder.Services.AddDbContext<SMSanagement_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Test_Con")));

// Add SignalR
builder.Services.AddSignalR();

// Register services for Dependency Injection (DI)
builder.Services.AddScoped<IPrinterRepository, PrinterRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<ProductHub>();
builder.Services.AddSingleton<SubscribeProductTableDependency>();
builder.Services.AddSingleton<SubscribeSaleTableDependency>();
builder.Services.AddSingleton<SubscribeCustomerTableDependency>();
builder.Services.AddSingleton<SubscribeShoppingProductTableDependency>();

// Configure Notification Service
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

// Build the app before configuring middleware
var app = builder.Build();
var connectionString = app.Configuration.GetConnectionString("Test_Con");

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

// Configure SignalR route and Map controller route
app.MapHub<DashboardHub>("/dashboardHub");
app.MapHub<ProductHub>("/productHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure SQL Table Dependency middleware
app.UseSqlTableDependency<SubscribeProductTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeSaleTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeCustomerTableDependency>(connectionString);
app.UseSqlTableDependency<SubscribeShoppingProductTableDependency>(connectionString);

// Run the application
app.Run();
