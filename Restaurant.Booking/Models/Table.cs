using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Booking.Models
{
    public class Table
    {
        private readonly object _locker = new object();
        public State State { get; private set; }
        public int SeatCount { get; }
        public int Id { get; }

        public Table(int id)
        {
            Id = id;
            State = State.Free;
            SeatCount = new Random().Next(2, 5);
        }

        public bool SetState(State state)
        {
            lock (_locker)
            {
                if (state == State)
                    return false;
                State = state;
                return true;
            }           
        }
    }
}
