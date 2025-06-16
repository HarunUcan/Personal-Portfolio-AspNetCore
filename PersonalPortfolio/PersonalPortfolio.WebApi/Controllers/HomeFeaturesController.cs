using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.HomeFeatureDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeFeaturesController : ControllerBase
    {
        private readonly ApiContext _context;

        public HomeFeaturesController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetHomeFeatures()
        {
            var homeFeatures = _context.HomeFeatures.FirstOrDefault();
            if (homeFeatures == null)
            {
                return NotFound("No home features found.");
            }

            return Ok(homeFeatures);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateHomeFeature(CreateHomeFeatureDto createHomeFeatureDto)
        {
            if (createHomeFeatureDto == null)
            {
                return BadRequest("Home feature cannot be null.");
            }

            byte[] bgImage = null;
            if (createHomeFeatureDto.BgImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    createHomeFeatureDto.BgImage.CopyTo(memoryStream);
                    bgImage = memoryStream.ToArray();
                }
            }

            var newFeature = new HomeFeature
            {
                Name = createHomeFeatureDto.Name,
                LastName = createHomeFeatureDto.LastName,
                Description = createHomeFeatureDto.Description,
                BgImage = bgImage != null ? Convert.ToBase64String(bgImage) : null,
                BgImageContentType = createHomeFeatureDto.BgImage?.ContentType
            };

            _context.HomeFeatures.Add(newFeature);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetHomeFeatures), new { id = newFeature.HomeFeatureId }, newFeature);
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateHomeFeatures(UpdateHomeFeatureDto updateHomeFeatureDto)
        {
            if (updateHomeFeatureDto == null)
            {
                return BadRequest("Home feature cannot be null.");
            }

            var existingFeature = _context.HomeFeatures.FirstOrDefault();
            if (existingFeature == null)
            {
                // oluştur
                CreateHomeFeatureDto createHomeFeatureDto = new CreateHomeFeatureDto
                {
                    Name = updateHomeFeatureDto.Name,
                    LastName = updateHomeFeatureDto.LastName,
                    Description = updateHomeFeatureDto.Description,
                    BgImage = updateHomeFeatureDto.BgImage
                };
                return CreateHomeFeature(createHomeFeatureDto);
            }

            byte[] bgImage = null;
            if (updateHomeFeatureDto.BgImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    updateHomeFeatureDto.BgImage.CopyTo(memoryStream);
                    bgImage = memoryStream.ToArray();
                }
            }

            existingFeature.Name = updateHomeFeatureDto.Name;
            existingFeature.LastName = updateHomeFeatureDto.LastName;
            existingFeature.Description = updateHomeFeatureDto.Description;

            existingFeature.BgImage = bgImage != null ? Convert.ToBase64String(bgImage) : existingFeature.BgImage;
            existingFeature.BgImageContentType = updateHomeFeatureDto.BgImage?.ContentType ?? existingFeature.BgImageContentType;

            _context.HomeFeatures.Update(existingFeature);

            _context.SaveChanges();

            return Ok(existingFeature);

        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteHomeFeature()
        {
            var homeFeature = _context.HomeFeatures.FirstOrDefault();
            if (homeFeature == null)
            {
                return NotFound("Home feature not found.");
            }

            _context.HomeFeatures.Remove(homeFeature);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
