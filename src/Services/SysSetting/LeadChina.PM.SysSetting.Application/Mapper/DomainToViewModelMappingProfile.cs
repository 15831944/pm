using AutoMapper;
using LeadChina.PM.SysSetting.Domain;
using LeadChina.PM.SysSetting.Domain.ViewModel;

namespace LeadChina.PM.SysSetting.Application.Mapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Account, AccountViewModel>();
        }
    }
}
