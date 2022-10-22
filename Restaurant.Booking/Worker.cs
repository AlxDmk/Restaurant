using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;
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
                Console.WriteLine("Привет! желаете забронировать столик?\n1 - мы уведомим Вас по смс (асинхронно)\n2 - подождите на линии, мы вас оповестим (синхронно)");

                if (!int.TryParse(Console.ReadLine(), out int choice) || (choice - 1) * (2 - choice) < 0)
                {
                    Console.WriteLine("Введите, пожалуйста от 1 до 2");
                    continue;
                }        

                await _bus.Publish((IBookingRequest) new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), Dish.Pizza, DateTime.Now, choice),stoppingToken);

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
