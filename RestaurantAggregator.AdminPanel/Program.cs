using Microsoft.EntityFrameworkCore;
using RestaurantAggregator.AdminPanel.BL.Services;
using RestaurantAggregator.AdminPanel.Common.Interfaces;
using RestaurantAggregator.API.BL.Services;
using RestaurantAggregator.API.Common.Interfaces;
using RestaurantAggregator.API.DAL;
using RestaurantAggregator.APIAuth.Middlewares;
using RestaurantAggregator.AuthApi.BL.Services;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//configure Databases
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("ConnectionBackend"))
);
builder.Services.AddDbContext<AuthDBContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("ConnectionAuth"))
);

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IAdminRestaurantsService, AdminRestaurantsService>();
builder.Services.AddScoped<IAdminProfileService, AdminProfileService>();
builder.Services.AddScoped<IAdminUsersServices, AdminUsersServices>();

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
    pattern: "{controller=Login}/{action=Login}/{id?}"
    );

app.Run();