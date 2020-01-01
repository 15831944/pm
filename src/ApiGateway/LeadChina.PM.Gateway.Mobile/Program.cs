using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace LeadChina.ProjectManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ÉèÖÃÅäÖÃÎÄ¼þ
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("ocelot.json", true, false);

            var hostingconfig = configurationBuilder.Build();
            var url = hostingconfig["serveraddress"];

            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });
            builder.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(hostingconfig)
                .UseIISIntegration()
                .UseMetricsWebTracking()
                .UseMetricsEndpoints()
                .UseUrls(url)
                .UseStartup<Startup>();
            var host = builder.Build();
            host.Run();
        }
    }
}
