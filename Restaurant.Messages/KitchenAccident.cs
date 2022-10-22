using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Messages
{
    public class KitchenAccident : IKitchenAccident
    {
        public KitchenAccident(Dish dish)
        {
           
            Dish = dish;

        }
        

        public Dish Dish { get; }

        
    }
}
