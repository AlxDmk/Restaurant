using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Kitchen
{
    public class Cook : BackgroundService
    {
        private readonly Manager _manager;
        private List<Dish> dishes = Enum.GetValues(typeof(Dish)).Cast<Dish>().ToList<Dish>();

        public Cook(Manager manager) =>
            _manager = manager;

        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                foreach (var dish in dishes)
                {
                    var ability = new Random().Next(0, 3);
                    Console.WriteLine($"{dish}  --> {ability}");
                    if (ability == 0 && dish == Dish.Pizza)
                        _manager.CheckKitchenAccident(dish);
                }
                await Task.Delay(2000, stoppingToken);

            }

        }
    }
}
