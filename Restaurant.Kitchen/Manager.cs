using MassTransit;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Kitchen
{
    public class Manager
    {
        private readonly IBus _bus;
     

        public Manager(IBus bus)
        {
            _bus = bus;
        }

        public void CheckKitchenReady(Guid orderId, Dish? dish)
        {            
            _bus.Publish<IKitchenReady>(new KitchenReady(orderId, true));
            
        }

        public void CheckKitchenAccident(Dish dish)
        {
            _bus.Publish<IKitchenAccident>(new KitchenAccident(dish));
        }
    }
}
