using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AdminPanel.BL.Services;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.API.BL.Services;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.APIAuth.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//configure Database
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("ConnectionBackend"))
);

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IRestaurantCrudService, RestaurantCrudService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();