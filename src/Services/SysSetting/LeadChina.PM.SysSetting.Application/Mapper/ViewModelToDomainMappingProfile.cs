using AutoMapper;
using LeadChina.PM.SysSetting.Domain;
using LeadChina.PM.SysSetting.Domain.Dto;

namespace LeadChina.PM.SysSetting.Application.Mapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AccountDto, Account>();
        }
    }
}
