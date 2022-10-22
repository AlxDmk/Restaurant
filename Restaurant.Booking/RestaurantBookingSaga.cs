using MassTransit;
using Restaurant.Booking.Consumers;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Booking
{
    public sealed class RestaurantBookingSaga :MassTransitStateMachine<RestaurantBooking>
    {
        public State AwaitingBookingApproved { get; private set; }

        public Event<IBookingRequest> BookingRequested { get; private set; }

        public Event<ITableBooked> TableBooked { get; private set; }

        public Event<IKitchenReady> KitchenReady { get; private set; }

        public Schedule<RestaurantBooking, IBookingExpire> BookingExpired { get; private set; }

        public Event BookingApproved { get; private set; }

        public RestaurantBookingSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => BookingRequested,
                x =>
                x.CorrelateById(context => context.Message.OrderId)
                .SelectId(context => context.Message.OrderId));

            Event(() => TableBooked,
                x =>
                x.CorrelateById(context => context.Message.OrderId));

            Event(() => KitchenReady,
                x =>
                x.CorrelateById(context => context.Message.OrderId));

            CompositeEvent(() => BookingApproved, 
                x => x.ReadyEventStatus, KitchenReady, TableBooked);

            Schedule(() => BookingExpired, 
                x => x.ExpirationId, x =>
                {
                    x.Delay = TimeSpan.FromSeconds(3);
                    x.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });

            Initially(
                When(BookingRequested)
                .Publish(c => (INotify)new Notify(c.Message.OrderId, c.Message.ClientId, "Пришел запрос на заказ столика"))
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.OrderId;
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.ClientId = context.Data.ClientId;

                })
                .Schedule(BookingExpired, context => new BookingExpire(context.Saga))
                .TransitionTo(AwaitingBookingApproved)
                );

            During(AwaitingBookingApproved,

                When(BookingApproved)
                .Unschedule(BookingExpired)
                .Publish(context =>
                (INotify)new Notify(
                    context.Instance.OrderId,
                    context.Instance.ClientId,
                    $"Стол успешно забронирован"))
                .Finalize());

            During(AwaitingBookingApproved,
                When(BookingExpired?.Received)                
                .Publish(context => (INotify)new Notify(context.Instance.OrderId,
                    context.Instance.ClientId,
                    $"Заказ отменен"))
                .Finalize()
                );            

            SetCompletedWhenFinalized();
            
        }
    }
}
