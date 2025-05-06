using BethanysPieShop.Web.Models;
using Microsoft.EntityFrameworkCore;

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = $"Server=localhost,1433;Database=BethanysPieShopDB;User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;MultipleActiveResultSets=true;";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BethanysPieShopDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();

DbInitializer.Seed(app);
app.Run();
