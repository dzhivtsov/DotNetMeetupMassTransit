using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.Saga;
using MassTransitDemo.SagaDemo.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MassTransitDemo.SagaDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<SagaDbContext<ShoppingCart, ShoppingCartMap>>(options =>
            {
                options.UseSqlServer(
                        @"Server=(localdb)\mssqllocaldb;Database=SagaDemo;Trusted_Connection=True;ConnectRetryCount=0",
                        sqlServerOptions =>
                            sqlServerOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName))
                    ;
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);

            services.AddTransient<ISagaDbContextFactory<ShoppingCart>>(serviceProvider =>
                new DelegateSagaDbContextFactory<ShoppingCart>(serviceProvider.GetRequiredService<SagaDbContext<ShoppingCart, ShoppingCartMap>>));
            services.AddTransient<ISagaRepository<ShoppingCart>, EntityFrameworkSagaRepository<ShoppingCart>>();
            services.AddTransient<ProcessPaymentConsumer>();
            services.AddTransient<ShoppingCartSaga>();
            services.AddMassTransit(serviceProvider =>
                
                Bus.Factory.CreateUsingInMemory(busConfigurator =>
                {
                    var cont = serviceProvider.GetRequiredService<SagaDbContext<ShoppingCart, ShoppingCartMap>>();
                    busConfigurator.ReceiveEndpoint("cart-saga", endpointConfigurator =>
                    {
                        endpointConfigurator.StateMachineSaga(serviceProvider.GetRequiredService<ShoppingCartSaga>(),
                            serviceProvider.GetRequiredService<ISagaRepository<ShoppingCart>>());
                    });

                    busConfigurator.ReceiveEndpoint("cart", endpointConfigurator =>
                    {
                        endpointConfigurator.Consumer<ProcessPaymentConsumer>(serviceProvider);
                    });

                    busConfigurator.UseInMemoryScheduler();
                })
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MassTransit Saga Demo API", Version = "v1" });
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
