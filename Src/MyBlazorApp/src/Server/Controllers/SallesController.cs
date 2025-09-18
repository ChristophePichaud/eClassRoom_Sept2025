using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SallesController : ControllerBase
    {
        private readonly SalleDeFormationService _service;

        public SallesController(SalleDeFormationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SalleDeFormationDto>>> GetAll()
        {
            var salles = await _service.GetAllAsync();
            return Ok(salles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalleDeFormationDto>> Get(int id)
        {
            var salle = await _service.GetByIdAsync(id);
            if (salle == null) return NotFound();
            return Ok(salle);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SalleDeFormationDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SalleDeFormationDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
