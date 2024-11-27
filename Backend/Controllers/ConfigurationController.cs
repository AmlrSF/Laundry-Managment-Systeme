using Laverie.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Laverie.Domain.Entities;

namespace Laverie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationService _configurationService;

        public ConfigurationController(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        // GET: api/configuration
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetConfiguration()
        {
            var configurations = await _configurationService.GetConfig();
            if (configurations == null || configurations.Count == 0)
            {
                return NotFound("No configurations found.");
            }
            return Ok(configurations);
        }


        [HttpPut("toggle-machine/{id}")]
        public async Task<IActionResult> ToggleMachine(int id)
        {
            var success = await _configurationService.ToggleMachineAsync(id);

            if (!success)
            {
                return NotFound(new { Message = "Machine not found or failed to toggle status." });
            }

            return Ok(new { Message = "Machine status toggled successfully." });
        }

        [HttpPost("addCycle")]
        public async Task<ActionResult> AddCycle([FromBody] Cycle cycle)
        {
            if (cycle == null || cycle.machineId <= 0 || cycle.price <= 0)
            {
                return BadRequest("Invalid cycle data.");
            }

            await _configurationService.AddCycleAsync(cycle);
            return CreatedAtAction(nameof(AddCycle), new { id = cycle.id }, cycle);
        }
    }
}
