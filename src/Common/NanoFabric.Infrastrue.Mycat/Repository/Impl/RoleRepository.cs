using LeadChina.ProjectManager.SysSetting.Entity;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    public class RoleRepository : Repository<Role, int>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
