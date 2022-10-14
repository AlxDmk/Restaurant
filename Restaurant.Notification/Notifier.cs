using MassTransit;
using MassTransit.DependencyInjection;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;
using System.Collections.Concurrent;

namespace Restaurant.Notification
{

    public class Notifier
    {
       public void Notify(Guid orderId, Guid clientId, string message)
        {
            Console.WriteLine($"[OrderId: {orderId}]  Уважаемый клиент {clientId}!  {message}");
        }
    }
}
