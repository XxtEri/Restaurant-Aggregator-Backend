using Microsoft.AspNetCore.SignalR;

namespace Notifications.BL.Hubs;

public class NotificationHub: Hub
{
    // public Task Send(string message)
    // {
    //     return Clients.All.SendAsync("ReceiveMessage", message);
    // }
}