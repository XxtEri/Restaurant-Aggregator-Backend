using Microsoft.AspNetCore.SignalR;
using Notifications.BL.Hubs;
using Notifications.Common.Interfaces;

namespace Notifications.BL.Services;

public class NotificationService: INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendNotification(string message)
    {
        await _hubContext.Clients.All.SendAsync("SendMessage", message);
    }
}