namespace Notifications.Common.Interfaces;

public interface INotificationService
{
    Task SendNotification(string notification);
}