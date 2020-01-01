using LeadChina.PM.SysSetting.Infrastrue.Repository;
using entity = LeadChina.PM.SysSetting.Domain;

namespace LeadChina.PM.SysSetting.Infrastrue.Repository
{
    public class OrgRepository : Repository<entity.Org, int>, IOrgRepository
    {
        public OrgRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
