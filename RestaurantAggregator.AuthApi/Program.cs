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


// builder.Services.AddAuthentication(options => {
      // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//      .AddJwtBearer(options =>
//      {
          // options.TokenValidationParameters = new TokenValidationParameters
          // {
          //     ValidateIssuer = false,
          //     ValidateAudience = false,
          //     ValidateLifetime = true,
          //     ValidateIssuerSigningKey = true,
          //     ValidIssuer = builder.Configuration["JWT:Issuer"]!,
          //     ValidAudience = builder.Configuration["JWT:Audience"]!,
          //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
          // };
//     });
//
// builder.Services.AddAuthorization(options => options.DefaultPolicy = 
//     new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
//         .RequireAuthenticatedUser()
//         .Build()
// );

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pathnostics", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
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