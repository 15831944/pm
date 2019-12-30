using LeadChina.ProjectManager.SysSetting.Entity;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    public class AccountRepository : Repository<Account, int>, IAccountRepository
    {
        public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
