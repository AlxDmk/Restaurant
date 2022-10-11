using Messaging;
using Restaurant.Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Restaurant.Booking
{
    internal class Restaurant
    {
        private readonly List<Table> _tables = new();        
        private readonly object _locker = new();
        private readonly Producer _producer = new(queueName: "BookingNotification", hostName: "Localhost");

        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
                _tables.Add(new Table(i));

        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Оставайтесь на линии");

            var table = _tables.FirstOrDefault(t => t.SeatCount > countOfPersons && t.State == State.Free);

            Thread.Sleep(5000);
            table?.SetState(State.Booked);

            _producer.Send(table is null
              ? "К сожалению, сейчас все столики заняты"
              : $"Готово! Ваш столик номер {table.Id}");
            
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Вам придет уведомление!");

            Task.Run(async () =>
            {
                Table? table;

                lock (_locker)
                {
                    table = _tables.FirstOrDefault(t => t.SeatCount > countOfPersons && t.State == State.Free);
                    Thread.Sleep(5000);
                    table?.SetState(State.Booked);
                }

                _producer.Send(table is null
                ? "УВЕДОМЛЕНИЕ! К сожалению, сейчас все столики заняты"
                : $"УВЕДОМЛЕНИЕ! Готово! Ваш столик номер {table.Id}");

                await Task.CompletedTask;
            });

        }

        public void UnsetBooking(int id)
        {
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);
            table?.SetState(State.Free);

            _producer.Send(table is null
                ? $"Точно этот стлик? столик {id} не был забронирован!"
                : $" Готово! Со столика {table.Id} снята бронь");            
        }

        public void UnsetBookingAsync(int id)
        {
            Task.Run(async () =>
            {
                Table? table;
                lock (_locker)
                {
                    table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);
                    Thread.Sleep(5000);
                    table?.SetState(State.Free);
                }
                _producer.Send(table is null
                    ? $"УВЕДОМЛЕНИЕ!  Точно этот стлик? столик {id} не был забронирован!"
                    : $"УВЕДОМЛЕНИЕ!  Готово! Со столика {table.Id} снята бронь");


                await Task.CompletedTask;
            });
            

        }

        #region TIMER UNSET BOOKING    

        public async void UnsetBooking(object? sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(_tables, t => t.SetState(State.Free));
                _producer.Send("Вся бронь со столиков снята");

            });
        }

        #endregion
    }
}
