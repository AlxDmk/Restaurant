using Restaurant.Messages;
using System.Collections.Concurrent;

namespace Restaurant.Notification
{

    public class Notifier
    {

        private readonly ConcurrentDictionary<Guid, Tuple<Guid?, Accepted, Dish?>> _state = new();   

        public void Accept(Guid orderId, Accepted accepted,  Dish? dish, Guid? clientId = null)
        {
            
            _state.AddOrUpdate(orderId, new Tuple<Guid?, Accepted, Dish?>(clientId, accepted, dish), (guid, oldValue) =>
            new Tuple<Guid?, Accepted, Dish?>(oldValue.Item1 ?? clientId, accepted == Accepted.Rejected ? Accepted.Rejected : oldValue.Item2 | accepted, oldValue.Item3 ?? dish ));
            
            Notify(orderId);
        }

        public void Unset(Dish dish)
        {
            IEnumerable<Guid> guids = _state.Keys.Where(s => _state[s].Item3 == dish);
            foreach (var id in guids)
            {                
                Console.WriteLine($"Заказ {id} отменен всвязи с отсутствием на кухне {_state[id].Item3}!") ;
                _state.Remove(id, out _);
            }
        }

        private void Notify(Guid orderId)
        {
            var booking = _state[orderId];

            switch (booking.Item2)
            {
                case Accepted.All:
                    Console.WriteLine($"Успешно забронировано для клиента {booking.Item1}");                    
                    break;
                case Accepted.Rejected:
                    Console.WriteLine($"Гость {booking.Item1}, к сожалению, все столики заняты");
                    _state.Remove(orderId, out _);
                    break;
                case Accepted.Kitchen:
                case Accepted.Booking:                    
                    break;               
                    
                default:
                    throw new ArgumentOutOfRangeException();

            }
        }
    }
}
