using AutoMapper;
using LeadChina.ProjectManager.SysSetting.Entity;
using LeadChina.ProjectManager.SysSetting.Entity.Dto;

namespace LeadChina.ProjectManager.SysSetting.BusiProcess.Mapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AccountDto, Account>();
        }
    }
}
