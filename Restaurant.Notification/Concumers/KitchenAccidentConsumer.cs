using MassTransit;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Notification.Concumers
{
    public class KitchenAccidentConsumer : IConsumer<IKitchenAccident>
    {
        private readonly Notifier _notifier;

        public KitchenAccidentConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }

        public async Task Consume(ConsumeContext<IKitchenAccident> context)
        {            
            _notifier.Unset(context.Message.Dish);
            await Task.CompletedTask;
        }
    }
}
