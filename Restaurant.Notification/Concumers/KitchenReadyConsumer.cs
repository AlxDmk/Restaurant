using MassTransit;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Notification.Concumers
{
    public class KitchenReadyConsumer : IConsumer<IKitchenReady>
    {
        private readonly Notifier _notifier;

        public KitchenReadyConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }
        public Task Consume(ConsumeContext<IKitchenReady> context)
        {
            var result = context.Message.Ready;            
            _notifier.Accept(context.Message.OrderId, result ? Accepted.Kitchen: Accepted.Rejected, 0);
            return Task.CompletedTask;
        }
    }
}
