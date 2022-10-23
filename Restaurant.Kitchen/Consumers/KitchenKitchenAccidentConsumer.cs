using MassTransit;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Kitchen.Consumers
{
    public class KitchenKitchenAccidentConsumer : IConsumer<Fault<IKitchenAccident>>
    {
        public Task Consume(ConsumeContext<Fault<IKitchenAccident>> context)
        {
            throw new NotImplementedException();
        }
    }
}
