using MassTransit;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Booking.Consumers
{
    public class RestaurantBookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        public RestaurantBookingRequestConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }
        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {           
            var choice = context.Message.MethodOfBooking;
            var result = choice switch
            {
                1 => await _restaurant.BookFreeTableAsync(1),
                2 => _restaurant.BookFreeTable(1),
                //3 => await _restaurant.UnsetBookingAsync(),
                //4 => _restaurant.UnsetBooking(),
                _ => throw new NotImplementedException()
            };
            
            await context.Publish((ITableBooked)new TableBooked(context.Message.OrderId, context.Message.ClientId, result ?? false, context.Message.PreOrder));
        }
    }
}
