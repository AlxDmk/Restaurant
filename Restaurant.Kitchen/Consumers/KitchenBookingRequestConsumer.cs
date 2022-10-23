using MassTransit;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Kitchen.Consumers
{
    internal class KitchenBookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;

        public KitchenBookingRequestConsumer(Manager manager)
        {
            _manager = manager;
        }
        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            Console.WriteLine($"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");
            Console.WriteLine("Trying time: " + DateTime.Now);

            await Task.Delay(5000);

            if (_manager.CheckKitchenReady(context.Message.OrderId, context.Message.ClientId, context.Message.PreOrder))
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
            else throw new Exception($"На кухне не порядок, отсутствует {context.Message.PreOrder}") ;
            
        }        
    }
}
