using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discount.Grpc.Repositories;
using Microsoft.Extensions.Configuration;

namespace Discount.Grpc
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterPostgres(services);
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }

        private void RegisterPostgres(IServiceCollection services)
        {
            var server = Configuration[Constants.Postgres.Environment.Host] ?? Constants.Postgres.DefaultHost;
            var port = Configuration[Constants.Postgres.Environment.Port] ?? Constants.Postgres.DefaultPort.ToString();
            var database = Configuration[Constants.Postgres.Environment.Database];
            var user = Configuration[Constants.Postgres.Environment.User];
            var password = Configuration[Constants.Postgres.Environment.Password];

            services.AddScoped<IDiscountRepository>(_ =>
                new DiscountRepository(
                    $"Server={server}; Port={port}; Database={database}; User ID={user}; Password={password}"));
        }
    }
}
