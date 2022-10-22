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
    public class Restaurant
    {
        private readonly List<Table> _tables = new();       
       
        public Restaurant()
        {
            for (ushort i = 1; i <= 10; i++)
                _tables.Add(new Table(i));
        }

        public bool? BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.SeatCount > countOfPersons && t.State == State.Free);
            Thread.Sleep(5000);   
                
            return table?.SetState(State.Booked);
        }

        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Вам придет уведомление!");          
                              
            var table = _tables.FirstOrDefault(t => t.SeatCount > countOfPersons && t.State == State.Free);
            await Task.Delay(5000);
            return table?.SetState(State.Booked);             
        }

        public bool? UnsetBooking()
        {
            Console.WriteLine("Введите номер столика");
            var id = Convert.ToInt16(Console.ReadLine());
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);
            return table?.SetState(State.Free);     
        }

        public async Task<bool?> UnsetBookingAsync()
        {
            var id = Convert.ToInt16(Console.ReadLine());
            var table = _tables.FirstOrDefault(t => t.Id == id && t.State == State.Booked);
            await Task.Delay(5000);
            return table?.SetState(State.Free);          

        }

        #region TIMER UNSET BOOKING    

        public async void UnsetBooking(object? sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(_tables, t => t.SetState(State.Free));
                //_producer.Send("Вся бронь со столиков снята");

            });
        }

        #endregion
    }
}
