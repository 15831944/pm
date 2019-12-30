using entity = LeadChina.ProjectManager.SysSetting.Entity;

namespace NanoFabric.Infrastrue.Mycat.Repository
{
    public class OrgRepository : Repository<entity.Org, int>, IOrgRepository
    {
        public OrgRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
