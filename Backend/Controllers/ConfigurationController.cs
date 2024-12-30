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


        [HttpPost("startMachine")]
        public async Task<IActionResult> startMachine([FromBody] Cycle cycle)
        {
            var success = await _configurationService.starteMachineAsync(cycle);

            if (!success)
            {
                return NotFound(new { Message = "Machine not found or failed to toggle status." });
            }

            return Ok(new { Message = "Machine status toggled successfully." });
        }


        [HttpPost("stopMachine")]
        public async Task<IActionResult> stopMachine([FromBody] Cycle cycle)
        {
            var success = await _configurationService.stopeMachineAsync(cycle);

            if (!success)
            {
                return NotFound(new { Message = "Machine not found or failed to toggle status." });
            }

            return Ok(new { Message = "Machine status toggled successfully." });
        }


    }
}
