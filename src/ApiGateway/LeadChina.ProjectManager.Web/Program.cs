using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace LeadChina.ProjectManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // …Ë÷√≈‰÷√Œƒº˛
                    var configurationBuilder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("ocelot.json", true, false);
                    var hostingconfig = configurationBuilder.Build();
                    var url = hostingconfig["serveraddress"];
                    webBuilder.UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseConfiguration(hostingconfig)
                        .UseIISIntegration()
                        .UseMetricsWebTracking()
                        .UseMetricsEndpoints()
                        .UseUrls(url)
                        .UseStartup<Startup>();
                });
    }
}
