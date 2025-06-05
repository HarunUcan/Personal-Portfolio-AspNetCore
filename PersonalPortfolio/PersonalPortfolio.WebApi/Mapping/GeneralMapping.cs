using AutoMapper;
using PersonalPortfolio.WebApi.Dtos.AboutDtos;
using PersonalPortfolio.WebApi.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebApi.Dtos.SkillDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<ProjectCategory,ResultProjectCategoryDto>().ReverseMap();
            CreateMap<ProjectCategory,CreateProjectCategoryDto>().ReverseMap();
            CreateMap<ProjectCategory,UpdateProjectCategoryDto>().ReverseMap();
            CreateMap<ProjectCategory,GetByIdProjectCategoryDto>().ReverseMap();

            CreateMap<About, CreateAboutDto>().ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore()) // çünkü dosya yüklemesini manuel yapacağız
                .ForMember(dest => dest.ProfileImageContentType, opt => opt.Ignore());
            CreateMap<About, UpdateAboutDto>().ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore()) // çünkü dosya yüklemesini manuel yapacağız
                .ForMember(dest => dest.ProfileImageContentType, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<About, ResultAboutDto>().ReverseMap();

            CreateMap<Skill, CreateSkillDto>().ReverseMap();
            CreateMap<Skill, UpdateSkillDto>().ReverseMap();
            CreateMap<Skill, ResultSkillDto>().ReverseMap();
            CreateMap<Skill, GetByIdSkillDto>().ReverseMap();
        }
    }
}
