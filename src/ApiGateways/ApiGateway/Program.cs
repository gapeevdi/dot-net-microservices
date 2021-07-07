using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                    {
                        configurationBuilder.AddJsonFile(
                            $"ocelot.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json");
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostBuilderContext, loggerBuilder) =>
                    {
                        loggerBuilder.AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"));
                        loggerBuilder.AddConsole();
                        loggerBuilder.AddDebug();
                    });
    }
}
