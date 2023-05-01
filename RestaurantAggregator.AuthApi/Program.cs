using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantAggregator.AuthApi.BL;
using RestaurantAggregator.AuthApi.BL.Services;
using RestaurantAggregator.AuthApi.Common.IServices;
using RestaurantAggregator.AuthApi.DAL.DBContext;
using RestaurantAggregator.AuthApi.DAL.Etities;
using RestaurantAggregator.CommonFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantAggregatorServiceAuth", Version = "v1" });
});

//configure Database
builder.Services.AddDbContext<AuthDBContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"))
);


//Configure Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
    options.Password.RequireNonAlphanumeric = false;
        
}).AddEntityFrameworkStores<AuthDBContext>();

//Add services
builder.Services.AddScoped<IAuthorizeServise, AuthorizeService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

//AuthJWTBearer
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtConfigs.Issuer,
            ValidAudience = JwtConfigs.Audience,
            IssuerSigningKey = JwtConfigs.GetSymmetricSecurityKey()
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await AuthConfiguration.SeedRoles(app.Services);

app.Run();