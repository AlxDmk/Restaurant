using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace Restaurant.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaraunt;

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            _restaraunt = restaurant;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Привет! желаете забронировать столик?\n1 - мы уведомим Вас по смс (асинхронно)\n2 - подождите на линии, мы вас оповестим (синхронно)\nЧтобы снять бронь со столика \n3 - мы уведомим вас по смс (асинхронно)\n4 - подождите на линии, мы вас оповестим (синхронно)");

                if (!int.TryParse(Console.ReadLine(), out int choice) || (choice - 1) * (4 - choice) < 0)
                {
                    Console.WriteLine("Введите, пожалуйста от 1 до 4");
                    continue;
                }               

                //var result = await _restaraunt.BookFreeTableAsync(1);


                var result = choice switch
                {
                    1 => await _restaraunt.BookFreeTableAsync(1),
                    2 => _restaraunt.BookFreeTable(1),
                    3 => await _restaraunt.UnsetBookingAsync(),
                    4 => _restaraunt.UnsetBooking()
                };

                await _bus.Publish(new TableBooked(NewId.NextGuid(), NewId.NextGuid(), result ?? false, Dish.Pizza),stoppingToken);

                Console.WriteLine("Спасибо за Ваш заказ!");               
            }

            //#region TIMER UNSET BOOKING

            //var timer = new Timer(20000);
            //timer.Elapsed += _restaraunt.UnsetBooking;
            //timer.AutoReset = true;
            //timer.Enabled = true;

            //#endregion
        }
    }
}
