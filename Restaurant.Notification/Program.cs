using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Notification.Concumers;

namespace Restaurant.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<KitchenAccidentConsumer>();
                    x.AddConsumer<TableBookedConsumer>();
                    x.AddConsumer<KitchenReadyConsumer>();

                    x.UsingRabbitMq((cxt, cfg) =>
                    {
                        cfg.Host("sparrow.rmq.cloudamqp.com", "yidbynwz", settings =>
                        {
                            settings.Username("yidbynwz");
                            settings.Password("SZP0QrAVJ0rOGQ028Ou7fwP-xHgqJouA");

                        });                   
                        
                       //TODO RetryPolicy

                        cfg.ConfigureEndpoints(cxt);
                    });
                });
              
                services.AddSingleton<Notifier>();                
                services.AddMassTransitHostedService(true);
                
                
            });
    }
}


