using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitDemo.OrderProcessor
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<IWarehouseService, WarehouseService>();

            services.AddMassTransit(massTransitConfigurator =>
            {
                massTransitConfigurator.AddConsumer<OrderSubmittedConsumer>();

                massTransitConfigurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(busConfigurator =>
                {
                    IRabbitMqHost host = busConfigurator.Host("localhost", "demo2", hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });
                    
                    busConfigurator.ReceiveEndpoint(host, "orders", e =>
                    {
                        e.PrefetchCount = 16;
                        e.ConfigureConsumer<OrderSubmittedConsumer>(provider);
                    });
                }));
            });
            
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            var busControl = serviceProvider.GetRequiredService<IBusControl>();
            await busControl.StartAsync();

            Console.WriteLine("Press any key to stop the bus");
            Console.ReadKey();

            await busControl.StopAsync();
        }
    }
}
