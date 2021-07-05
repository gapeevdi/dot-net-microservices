using System;
using Basket.API.Discount;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Basket.API
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

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Catalog.API",
                    Version = "v1"
                });
            });

            ConfigureRedis(services);
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options=>options.Address = new Uri(Configuration["DISCOUNT_GRPC_URL"]));

            ConfigureMassTransit(services);
            services.AddMassTransitHostedService();

            services.AddSingleton<IDiscountClient, DiscountClient>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureRedis(IServiceCollection collection)
        {
            var redisHost = Configuration[Constants.Redis.Environment.Host] ?? Constants.Redis.DefaultHost;
            var redisPort = Configuration[Constants.Redis.Environment.Port] ?? Constants.Redis.DefaultPort.ToString();

            collection.AddStackExchangeRedisCache(redisOptions =>
            {
                redisOptions.Configuration = $"{redisHost}:{redisPort}";
            });
        }

        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, rabbitMqConfigurator) =>
                {
                    rabbitMqConfigurator.Host("EventBusSettings:HostAddress");
                });
            });
        }
    }
}
