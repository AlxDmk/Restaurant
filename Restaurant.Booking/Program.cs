using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Booking.Consumers;
using System.Runtime.CompilerServices;

namespace Restaurant.Booking
{
    public static class Program
    {        
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
                   
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    
                    x.AddDelayedMessageScheduler();

                    x.AddConsumer<RestaurantBookingRequestConsumer>();                   
                    x.AddConsumer<RestauranrBookingRequestFaultConsumer>();
                   

                    x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()                    
                    .InMemoryRepository();
                    

                    x.UsingRabbitMq((context, conf) =>
                    {                       
                        conf.UseMessageRetry(r =>                        
                            r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)));                        

                        conf.UseScheduledRedelivery(r
                            => r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30)));

                        conf.UseDelayedMessageScheduler();
                        conf.UseInMemoryOutbox();
                        conf.ConfigureEndpoints(context);
                    });
                });

                services.AddTransient<RestaurantBooking>();
                services.AddTransient<RestaurantBookingSaga>();

                services.AddTransient<Restaurant>();

                services.AddHostedService<Worker>();
            });
            

    }
}
