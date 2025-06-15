using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.ProjectDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            //sadece IsMain olan resimler dahil edilecek
            var projects = _context.Projects
                .Include(p => p.ProjectImages.Where(pi => pi.IsMain))
                .Include(p => p.ProjectCategory)
                .ToList();
            if (projects == null || !projects.Any())
            {
                return NotFound("No projects found.");
            }

            // Assuming you have a mapping defined for Project to ResultProjectDto
            var resultProjects = _mapper.Map<List<ResultProjectDto>>(projects);


            return Ok(resultProjects);
        }

        [HttpGet("{id}")]
        public IActionResult GetProjectById(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectImages)
                .Include(p => p.ProjectCategory)
                .FirstOrDefault(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            // Assuming you have a mapping defined for Project to ResultProjectDto
            var resultProject = _mapper.Map<ResultProjectDto>(project);
            return Ok(resultProject);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateProject(CreateProjectDto createProjectDto)
        {
            if (createProjectDto == null)
            {
                return BadRequest("Project cannot be null.");
            }

            List<byte[]> uploadedImages = new List<byte[]>();
            if (createProjectDto.ProjectImages != null && createProjectDto.ProjectImages.Any())
            {
                foreach (var image in createProjectDto.ProjectImages)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);
                        uploadedImages.Add(memoryStream.ToArray());
                    }
                }
            }

            var newProject = _mapper.Map<Project>(createProjectDto);

            List<ProjectImage> projectImages = new List<ProjectImage>();

            for (int i = 0; i < uploadedImages.Count; i++)
            {
                projectImages.Add(new ProjectImage
                {
                    Url = Convert.ToBase64String(uploadedImages[i]),
                    IsMain = i == 0, // Set the first image as the main image
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            newProject.ProjectImages = projectImages;

            _context.Projects.Add(newProject);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProjects), new { id = newProject.ProjectId }, newProject);
        }

    }
}
