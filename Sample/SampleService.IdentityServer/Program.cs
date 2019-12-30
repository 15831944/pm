using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SampleService.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 设置配置文件
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
