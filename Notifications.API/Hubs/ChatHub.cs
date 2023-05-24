using Microsoft.AspNetCore.SignalR;

namespace Notifications.Hubs;

public class ChatHub: Hub
{
    public Task SendMessage(string message)
    {
        return Clients.Others.SendAsync("Send", message);
    }
}