using Microsoft.OpenApi.Models;
using Notifications.BL.Hubs;
using Notifications.BL.Services;
using Notifications.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Configure services for Rabbit
services.AddSingleton<IRabbitMqService, RabbitMqService>();
services.AddSingleton<IReceiverService, ReceiverService>();
services.AddHostedService<ReceiverHostedService>();

//Configure SignalR
services.AddSingleton<NotificationHub>();
services.AddSignalR();

//Configure other Services
services.AddSingleton<INotificationService, NotificationService>();

//Configure CORS
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("null");
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notification");

app.Run();
