using Catalog.API.Data;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Catalog.API
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

            RegisterMongoDb(services);
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API "));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterMongoDb(IServiceCollection serviceCollection)
        {
            var mongodbHost = Configuration[Constants.Mongo.Environment.Host] ?? Constants.Mongo.DefaultHost;
            var mongodbPort = int.Parse(Configuration[Constants.Mongo.Environment.Port] ?? Constants.Mongo.DefaultPort.ToString());
            var database = Configuration[Constants.Mongo.Environment.ProductDatabase];
            var collection = Configuration[Constants.Mongo.Environment.ProductCollection];

            serviceCollection.AddScoped<ICatalogContext>(provider =>
                new CatalogContext($"mongodb://{mongodbHost}:{mongodbPort}", database, collection));
        }
    }
}
