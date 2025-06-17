using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos;
using PersonalPortfolio.WebApi.Dtos.ProjectCategoryDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectCategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        public ProjectCategoriesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProjectCategories()
        {
            var projectCategories = _context.ProjectCategories.ToList();
            if (projectCategories == null || !projectCategories.Any())
            {
                return NotFound("No project categories found.");
            }

            return Ok(_mapper.Map<List<ResultProjectCategoryDto>>(projectCategories));
        }

        [HttpGet("{id}")]
        public IActionResult GetProjectCategoryById(int id)
        {
            var projectCategory = _context.ProjectCategories.Find(id);
            if (projectCategory == null)
            {
                return NotFound("Project category not found.");
            }

            return Ok(_mapper.Map<GetByIdProjectCategoryDto>(projectCategory));
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateProjectCategory(CreateProjectCategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Project category cannot be null.");
            }

            var projectCategory = _mapper.Map<ProjectCategory>(categoryDto);

            _context.ProjectCategories.Add(projectCategory);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Project category created successfully.",
                projectCategoryId = projectCategory.ProjectCategoryId
            });

        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateProjectCategory(int id, UpdateProjectCategoryDto updateProjectCategoryDto)
        {
            if (updateProjectCategoryDto == null)
            {
                return BadRequest("Project category cannot be null.");
            }

            var projectCategory = _context.ProjectCategories.Find(id);
            if (projectCategory == null)
            {
                return NotFound("Project category not found.");
            }

            projectCategory = _mapper.Map(updateProjectCategoryDto, projectCategory);
            projectCategory.UpdatedAt = DateTime.UtcNow;

            _context.ProjectCategories.Update(projectCategory);
            _context.SaveChanges();

            return Ok("Project category updated successfully.");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteProjectCategory(int id)
        {
            var projectCategory = _context.ProjectCategories.Find(id);
            if (projectCategory == null)
            {
                return NotFound("Project category not found.");
            }

            _context.ProjectCategories.Remove(projectCategory);
            _context.SaveChanges();

            return Ok("Project category deleted successfully.");
        }
    }
}
