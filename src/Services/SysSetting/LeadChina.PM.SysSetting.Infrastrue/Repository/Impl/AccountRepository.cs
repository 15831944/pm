using LeadChina.PM.SysSetting.Domain;

namespace LeadChina.PM.SysSetting.Infrastrue.Repository
{
    public class AccountRepository : Repository<Account, int>, IAccountRepository
    {
        public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
