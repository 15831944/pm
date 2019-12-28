using DnsClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.Router;
using NanoFabric.Router.Consul;
using NanoFabric.Router.Throttle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

internal class Program
{
    private readonly IDnsQuery _dns;
    private static ServiceProvider _serviceProvider;

    private static void Main(string[] args)
    {
        
        Console.ReadLine();
    }

    private static void Initialize()
    {
        // 加载配置文件
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        _serviceProvider = new ServiceCollection()
            .AddNanoFabricConsulRouter(configuration) // 添加微服务路由中间件
            .AddLogging() // 添加日志中间件
            .BuildServiceProvider();
    }

    private static void Demo1()
    {
        Initialize();
        IServiceSubscriberFactory subscriberFactory = _serviceProvider.GetRequiredService<IServiceSubscriberFactory>();
        // 创建ConsoleLogProvider并根据日志类目名称（CategoryName）生成Logger实例
        var logger = _serviceProvider.GetService<ILoggerFactory>().AddConsole().CreateLogger("App");
        // 创建服务订阅者
        var serviceSubscriber = subscriberFactory.CreateSubscriber("SampleService_Kestrel", 
            ConsulSubscriberOptions.Default, new ThrottleSubscriberOptions() { 
                MaxUpdatesPeriod = TimeSpan.FromSeconds(30), MaxUpdatesPerPeriod = 20 });
        // 启动订阅
        serviceSubscriber.StartSubscription().ConfigureAwait(false).GetAwaiter().GetResult();
        // 注册端口变更事件
        serviceSubscriber.EndpointsChanged += async (sender, eventArgs) =>
        {
            // 重置连接池，处理此信息等
            var endpoints = await serviceSubscriber.Endpoints();
            var servicesInfo = string.Join(",", endpoints);
            logger.LogInformation($"已接收更新的订阅者：[{servicesInfo}]");
        };
        // 创建轮播负载均衡
        ILoadBalancer loadBalancer = new RoundRobinLoadBalancer(serviceSubscriber);
        // 获取负载均衡服务的注册信息
        var endPoint = loadBalancer.Endpoint().ConfigureAwait(false).GetAwaiter().GetResult();
        // 创建 http 请求客户端
        var httpClient = new HttpClient();
        // 生成追踪Id
        var traceid = Guid.NewGuid().ToString();
        // http请求消息头中添加追踪标识
        httpClient.DefaultRequestHeaders.Add("ot-traceid", traceid);
        // 获取请求响应
        var content = httpClient.GetStringAsync($"{endPoint.ToUri()}api/values").Result;
        Console.WriteLine($"{traceid} 响应内容: { content }");
    }

    private static void Demo2()
    {
        var DB = new SampleContext();
        DB.Database.EnsureCreated();
        for (var i = 0; i < 50; i++)
            DB.Blogs.Add(new Blog { Title = "Hello", Time = DateTime.Now, Content = "MyCat" });
        DB.SaveChanges();
        Console.WriteLine(DB.Blogs.Count());
    }
}

private class Blog
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime Time { get; set; }

    public JsonObject<List<string>> Tags { get; set; }
}

private class SampleContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseMyCat("server=192.168.0.102;port=8066;uid=test;pwd=test;database=blog")
            .UseDataNode("192.168.0.102", "mycatblog1", "root", "19931101")
            .UseDataNode("192.168.0.102", "mycatblog2", "root", "19931101");
    }
}
