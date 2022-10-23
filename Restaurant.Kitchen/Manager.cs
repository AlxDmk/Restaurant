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
        
        public Manager(IBus bus) =>       
            _bus = bus;


        public bool CheckKitchenReady(Guid orderId, Guid clientId, Dish? dish) =>
           CheckKitchenAccident(dish);


        public bool CheckKitchenAccident(Dish? dish) =>
            dish != Dish.Lasagna;
        
    }
}
