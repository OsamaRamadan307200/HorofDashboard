using HorofDashboard.Models;
using HorofDashboard.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var connectionString = Environment.GetEnvironmentVariable("MSSQL_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("HorofContent");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HorofContentContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
