using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace LeadChina.ProjectManager.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // …Ë÷√≈‰÷√Œƒº˛
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var hostingconfig = configurationBuilder.Build();
            var url = hostingconfig["serveraddress"];

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(hostingconfig)
                .UseIISIntegration()
                .UseUrls(url)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
