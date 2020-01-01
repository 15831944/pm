using LeadChina.PM.SysSetting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LeadChina.PM.SysSetting.Infrastrue
{
    /// <summary>
    /// 项目管理系统数据库上下文
    /// </summary>
    public class LeadChinaPMDbContext : DbContext
    {
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 从 appsetting.json 中获取配置信息
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            // 设置 Mycat 连接字符串
            optionsBuilder.UseLazyLoadingProxies().UseMySQL(config.GetConnectionString("MycatConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
