using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Kitchen.Consumers;

namespace Restaurant.Kitchen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => {
                services.AddMassTransit(x =>    
                {
                    x.AddConsumer<KitchenBookingRequestConsumer>();
                    
                    x.UsingRabbitMq((context, conf) =>
                    {
                       conf.ConfigureEndpoints(context);
                    });
                    
                });

                services.AddSingleton<Manager>();
                services.AddHostedService<Cook>();
                

                
            });          
            
    }
}
