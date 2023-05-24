using Notifications.BL.Hubs;
using Notifications.BL.Services;
using Notifications.Common.Interfaces;
using Notifications.Hubs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

//Configure SignalR
services.AddSingleton<NotificationHub>();
services.AddSignalR();

//Configure Rabbit
services.AddSingleton<IRabbitMqService, RabbitMqService>();
services.AddSingleton<IReceiverService, ReceiverService>();
services.AddHostedService<ReceiverHostedService>();

//Configure Services
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


services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
