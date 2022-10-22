using MassTransit;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Notification.Concumers
{
    public class TableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Notifier _notifier;
        public TableBookedConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }
        public Task Consume(ConsumeContext<ITableBooked> context)
        {
            var result = context.Message.Success;
            _notifier.Accept(
                orderId: context.Message.OrderId,
                accepted: result ? Accepted.Booking : Accepted.Rejected,
                clientId: context.Message.ClientId,
                dish: Messages.Dish.Pizza);

            return Task.CompletedTask;

        }
    }
}
