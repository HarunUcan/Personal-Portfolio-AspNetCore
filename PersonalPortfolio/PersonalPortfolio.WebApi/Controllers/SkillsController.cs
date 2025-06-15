using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.WebApi.Context;
using PersonalPortfolio.WebApi.Dtos.SkillDtos;
using PersonalPortfolio.WebApi.Entities;

namespace PersonalPortfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public SkillsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetSkills()
        {
            var skills = _context.Skills.ToList();
            if (skills == null || !skills.Any())
            {
                return NotFound("No skills found.");
            }

            return Ok(skills);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateSkill(CreateSkillDto createSkillDto)
        {
            if (createSkillDto == null)
            {
                return BadRequest("Skill cannot be null.");
            }

            var newSkill = _mapper.Map<Skill>(createSkillDto);
            _context.Skills.Add(newSkill);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSkills), new { id = newSkill.SkillId }, newSkill);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateSkill(int id, UpdateSkillDto updateSkillDto)
        {
            if (updateSkillDto == null)
            {
                return BadRequest("Skill cannot be null.");
            }

            var existingSkill = _context.Skills.Find(id);
            if (existingSkill == null)
            {
                return NotFound($"Skill with ID {id} not found.");
            }

            _mapper.Map(updateSkillDto, existingSkill);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteSkill(int id)
        {
            var skill = _context.Skills.Find(id);
            if (skill == null)
            {
                return NotFound($"Skill with ID {id} not found.");
            }

            _context.Skills.Remove(skill);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetSkillById(int id)
        {
            var skill = _context.Skills.Find(id);
            if (skill == null)
            {
                return NotFound($"Skill with ID {id} not found.");
            }

            return Ok(skill);
        }
    }
}
