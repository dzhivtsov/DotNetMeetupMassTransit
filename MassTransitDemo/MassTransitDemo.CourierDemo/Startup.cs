using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransitDemo.CourierDemo.Activities.ApplyCoupon;
using MassTransitDemo.CourierDemo.Activities.Config;
using MassTransitDemo.CourierDemo.Activities.ProcessPayment;
using MassTransitDemo.CourierDemo.Activities.ReserveProduct;
using MassTransitDemo.CourierDemo.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MassTransitDemo.CourierDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<OrderSubmittedConsumer>();

            services.AddTransient<ReserveProductActivity>();
            services.AddTransient<ApplyCouponActivity>();
            services.AddTransient<ProcessPaymentActivity>();

            services.AddTransient<OrderProcessingCompletedConsumer>();
            services.AddTransient<RoutingSlipEventConsumer>();

            services.AddSingleton<IActivitiesConfig>(_ => new ActivitiesConfig("loopback://localhost", "orders"));

            services.AddMassTransit(serviceProvider =>
                Bus.Factory.CreateUsingInMemory(busConfigurator =>
                {
                    busConfigurator.ReceiveEndpoint("orders", endpointConfigurator =>
                    {
                        endpointConfigurator.Consumer<OrderSubmittedConsumer>(serviceProvider);
                        endpointConfigurator.Consumer<RoutingSlipEventConsumer>(serviceProvider);
                        endpointConfigurator.Consumer<OrderProcessingCompletedConsumer>(serviceProvider);
                    });

                    var activitiesConfig = serviceProvider.GetRequiredService<IActivitiesConfig>();

                    busConfigurator.ReceiveEndpoint(activitiesConfig.ReserveProduct.ExecuteQueueName, endpointConfigurator =>
                        {
                            endpointConfigurator.ExecuteActivityHost<ReserveProductActivity, ReserveProductArguments>(
                                activitiesConfig.ReserveProduct.CompensateEndpointUri, serviceProvider);
                        });
                    
                    busConfigurator.ReceiveEndpoint(activitiesConfig.ReserveProduct.CompensateQueueName, endpointConfigurator =>
                    {
                        endpointConfigurator.CompensateActivityHost<ReserveProductActivity, ReserveProductLog>(serviceProvider);
                    });

                    busConfigurator.ReceiveEndpoint(activitiesConfig.ApplyCoupon.ExecuteQueueName, endpointConfigurator =>
                    {
                        endpointConfigurator.ExecuteActivityHost<ApplyCouponActivity, ApplyCouponArguments>(serviceProvider);
                    });

                    busConfigurator.ReceiveEndpoint(activitiesConfig.ProcessPayment.ExecuteQueueName, endpointConfigurator =>
                        {
                            endpointConfigurator.ExecuteActivityHost<ProcessPaymentActivity, ProcessPaymentArguments>(
                                activitiesConfig.ProcessPayment.CompensateEndpointUri, serviceProvider);
                        });

                    busConfigurator.ReceiveEndpoint(activitiesConfig.ProcessPayment.CompensateQueueName, endpointConfigurator =>
                    {
                        endpointConfigurator.CompensateActivityHost<ProcessPaymentActivity, ProcessPaymentLog>(serviceProvider);
                    });

                })
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MassTransit Courier Demo API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MassTransit Demo API V1");
            });
        }
    }
}
