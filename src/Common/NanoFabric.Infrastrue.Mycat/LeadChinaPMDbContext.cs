using Microsoft.EntityFrameworkCore;

namespace NanoFabric.Infrastrue.Mycat
{
    /// <summary>
    /// 项目管理系统数据库上下文
    /// </summary>
    public class LeadChinaPMDbContext : DbContext
    {
        public LeadChinaPMDbContext(DbContextOptions<LeadChinaPMDbContext> options) : base(options)
        {

        }
    }
}
