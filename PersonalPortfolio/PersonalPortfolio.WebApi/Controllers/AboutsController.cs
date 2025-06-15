using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.AboutDtos;
using PersonalPortfolio.WebApi.Dtos.SkillDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public AboutsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAbouts()
        {
            var abouts = _context.Abouts.FirstOrDefault();
            if (abouts == null)
            {
                return NotFound("No abouts found.");
            }

            // Assuming you have a mapping defined for About to ResultAboutDto
            var resultAbout = _mapper.Map<ResultAboutDto>(abouts);
            return Ok(resultAbout);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            if (createAboutDto == null)
            {
                return BadRequest("About cannot be null.");
            }

            if(_context.Abouts.Any())
                return BadRequest("About already exists. You can only have one About entry.");
            byte[] profileImage = null;

            if (createAboutDto.ProfileImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    createAboutDto.ProfileImage.CopyTo(memoryStream);
                    profileImage = memoryStream.ToArray();
                }
            }

            var newAbout = _mapper.Map<About>(createAboutDto);

            newAbout.ProfileImage = profileImage != null
                ? Convert.ToBase64String(profileImage)
                : null; // Handle the case where no image is provided

            newAbout.ProfileImageContentType = createAboutDto.ProfileImage?.ContentType; // Store the content type if available

            _context.Abouts.Add(newAbout);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            if (updateAboutDto == null)
            {
                return BadRequest("About cannot be null.");
            }

            var existingAbout = _context.Abouts.FirstOrDefault();
            if (existingAbout == null)
            {
                return NotFound("About not found.");
            }

            byte[] profileImage = null;

            if (updateAboutDto.ProfileImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    updateAboutDto.ProfileImage.CopyTo(memoryStream);
                    profileImage = memoryStream.ToArray();
                }
            }

            // Map the updated properties
            _mapper.Map(updateAboutDto, existingAbout);

            existingAbout.ProfileImage = profileImage != null
                ? Convert.ToBase64String(profileImage)
                : existingAbout.ProfileImage; // Keep the existing image if no new one is provided

            existingAbout.ProfileImageContentType = updateAboutDto.ProfileImage?.ContentType ?? existingAbout.ProfileImageContentType; // Update content type if a new image is provided

            _context.Abouts.Update(existingAbout);

            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteAbout()
        {
            var about = _context.Abouts.FirstOrDefault();
            if (about == null)
            {
                return NotFound("About not found.");
            }

            _context.Abouts.Remove(about);
            _context.SaveChanges();

            return Ok("About deleted successfully.");
        }
    }
}
