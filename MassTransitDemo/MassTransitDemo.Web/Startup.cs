using System.Net.Mime;
using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;

namespace MassTransitDemo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHealthChecks();
            services.AddMassTransit(serviceProvider =>
                Bus.Factory.CreateUsingRabbitMq(busConfigurator =>
                {
                    IRabbitMqHost host = busConfigurator.Host("localhost", "demo2", hostConfigurator =>
                    {
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });
                })
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MassTransit Demo API", Version = "v1" });
            });
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health",
                new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = (context, report) =>
                    {
                        string result = JsonConvert.SerializeObject(report, Formatting.Indented, new StringEnumConverter());
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        return context.Response.WriteAsync(result);
                    }
                });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MassTransit Demo API V1");
            });
        }
    }
}
