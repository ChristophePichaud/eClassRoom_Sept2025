using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MachinesController : ControllerBase
    {
        private readonly MachineVirtuelleService _service;

        public MachinesController(MachineVirtuelleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MachineVirtuelleDto>>> GetAll()
        {
            var vms = await _service.GetAllAsync();
            return Ok(vms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MachineVirtuelleDto>> Get(int id)
        {
            var vm = await _service.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return Ok(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MachineVirtuelleDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MachineVirtuelleDto dto)
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
