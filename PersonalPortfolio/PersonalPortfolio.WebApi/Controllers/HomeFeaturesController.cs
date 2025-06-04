using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.HomeFeatureDtos;

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
                return NotFound("Home feature not found.");
            }

            existingFeature.Name = updateHomeFeatureDto.Name;
            existingFeature.LastName = updateHomeFeatureDto.LastName;
            existingFeature.Description = updateHomeFeatureDto.Description;

            if(updateHomeFeatureDto.BgImageUrl != null)
                existingFeature.BgImageUrl = updateHomeFeatureDto.BgImageUrl;

            _context.SaveChanges();

            return Ok(existingFeature);

        }
    }
}
