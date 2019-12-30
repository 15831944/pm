using LeadChina.ProjectManager.SysSetting.Entity.ViewModel;
using NanoFabric.Core;

namespace LeadChina.ProjectManager.SysSetting.BusiProcess
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IAccountService : IService<AccountViewModel, int>
    {
        /// <summary>
        /// 根据工号，密码获取账号
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AccountViewModel GetAcount(string username, string password);
    }
}
