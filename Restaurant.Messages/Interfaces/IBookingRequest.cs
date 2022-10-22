using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Messages.Interfaces
{
    public interface IBookingRequest
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public int MethodOfBooking { get; }
        public DateTime CreationDate { get; }
    }
}
