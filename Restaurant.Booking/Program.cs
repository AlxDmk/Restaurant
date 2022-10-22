using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    //x.AddConsumer<BookingKitchenReadyConsumer>();
                    x.UsingRabbitMq((context, conf) =>
                    {
                        conf.Host("sparrow.rmq.cloudamqp.com", "yidbynwz", settings =>
                        {
                            settings.Username("yidbynwz");
                            settings.Password("SZP0QrAVJ0rOGQ028Ou7fwP-xHgqJouA");

                        });
                        conf.ConfigureEndpoints(context);
                    });
                });

                services.AddMassTransitHostedService(true);

                services.AddTransient<Restaurant>();

                services.AddHostedService<Worker>();
            });
            

    }
}
