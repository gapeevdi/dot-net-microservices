using EventBus.Messages;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });

            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);
            ConfigureMassTransit(services);

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<BasketCheckoutConsumer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketCheckoutConsumer>(); 

                config.UsingRabbitMq((context, rabbitMqConfigurator) =>
                {
                    rabbitMqConfigurator.Host("EventBusSettings:HostAddress");

                    rabbitMqConfigurator.ReceiveEndpoint(Constants.Queues.BasketCheckoutQueue, endpointConfiguration =>
                        {
                            endpointConfiguration.ConfigureConsumer<BasketCheckoutConsumer>(context);
                        });
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
