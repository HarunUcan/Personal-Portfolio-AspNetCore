using AutoMapper;
using PersonalPortfolio.WebApi.Dtos.ProjectCategoryDtos;
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
        }
    }
}
