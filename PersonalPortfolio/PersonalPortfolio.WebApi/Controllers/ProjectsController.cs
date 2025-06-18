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
        public IActionResult CreateProject([FromForm]CreateProjectDto createProjectDto)
        {
            if (createProjectDto == null)
            {
                return BadRequest("Project cannot be null.");
            }

            List<string> uploadedImages = new List<string>();
            if (createProjectDto.ProjectImages != null && createProjectDto.ProjectImages.Any())
            {
                foreach (var image in createProjectDto.ProjectImages)
                {
                    uploadedImages.Add(image);
                }
            }

            var newProject = _mapper.Map<Project>(createProjectDto);

            List<ProjectImage> projectImages = new List<ProjectImage>();

            for (int i = 0; i < uploadedImages.Count; i++)
            {
                projectImages.Add(new ProjectImage
                {
                    Url = uploadedImages[i],
                    IsMain = i == 0, // Set the first image as the main image
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            newProject.ProjectImages = projectImages;

            _context.Projects.Add(newProject);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, [FromForm]UpdateProjectDto updateProjectDto)
        {
            if (updateProjectDto == null)
            {
                return BadRequest("Project cannot be null.");
            }

            var existingProject = _context.Projects
                .Include(p => p.ProjectImages)
                .FirstOrDefault(p => p.ProjectId == id);

            if (existingProject == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            // Update project properties
            existingProject.Name = updateProjectDto.Name;
            existingProject.Description = updateProjectDto.Description;
            existingProject.ProjectUrl = updateProjectDto.ProjectUrl;
            existingProject.ProjectCategoryId = updateProjectDto.ProjectCategoryId;
            existingProject.UpdatedAt = DateTime.UtcNow;

            // Handle images
            if (updateProjectDto.ProjectImages != null && updateProjectDto.ProjectImages.Any())
            {
                foreach (var image in updateProjectDto.ProjectImages)
                {
                    
                    var projectImage = new ProjectImage
                    {
                        Url = image,
                        IsMain = false, // Default to false, you can change this logic as needed
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    existingProject.ProjectImages.Add(projectImage);
                    
                }
            }

            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectImages)
                .FirstOrDefault(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteImage/{imageId}")]
        public IActionResult DeleteImage(int imageId)
        {
            var projectImage = _context.ProjectImages.FirstOrDefault(pi => pi.ProjectImageId == imageId);
            if (projectImage == null)
            {
                return NotFound($"Image with ID {imageId} not found.");
            }
            // If the image is the main image, we cannot delete it directly
            if (projectImage.IsMain)
            {
                return BadRequest("Cannot delete the main image. Please set another image as main before deleting this one.");
            }

            var imageUrl = projectImage.Url;
            _context.ProjectImages.Remove(projectImage);
            _context.SaveChanges();

            return Ok(new { message = "Image deleted successfully.", imageUrl });
        }

        [Authorize]
        [HttpPut("SetMainImage/{imageId}")]
        public IActionResult SetMainImage(int imageId)
        {
            var projectImage = _context.ProjectImages.FirstOrDefault(pi => pi.ProjectImageId == imageId);
            if (projectImage == null)
            {
                return NotFound($"Image with ID {imageId} not found.");
            }

            // Set all images to not main
            var allImages = _context.ProjectImages.Where(pi => pi.ProjectId == projectImage.ProjectId).ToList();
            foreach (var img in allImages)
            {
                img.IsMain = false;
            }

            // Set the selected image as main
            projectImage.IsMain = true;

            _context.SaveChanges();

            return Ok();
        }
    }
}
