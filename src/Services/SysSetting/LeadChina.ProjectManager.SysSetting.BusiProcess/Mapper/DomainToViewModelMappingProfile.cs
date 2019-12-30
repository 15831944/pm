using AutoMapper;
using LeadChina.ProjectManager.SysSetting.Entity;
using LeadChina.ProjectManager.SysSetting.Entity.ViewModel;

namespace LeadChina.ProjectManager.SysSetting.BusiProcess.Mapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Account, AccountViewModel>();
        }
    }
}
