using LeadChina.PM.SysSetting.Domain;

namespace LeadChina.PM.SysSetting.Infrastrue.Repository
{
    public class RoleRepository : Repository<Role, int>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
