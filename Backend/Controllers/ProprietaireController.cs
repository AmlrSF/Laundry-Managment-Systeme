using Laverie.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Laverie.Domain.Entities;
namespace Laverie.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProprietaireController : ControllerBase
    {
        private readonly ProprietaireService _service;

        public ProprietaireController(ProprietaireService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var proprietaire = _service.GetById(id);
            if (proprietaire == null) return NotFound();
            return Ok(proprietaire);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User proprietaire)
        {
            _service.Create(proprietaire);
            return CreatedAtAction(nameof(GetById), new { id = proprietaire.id }, proprietaire);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User proprietaire)
        {
            if (id != proprietaire.id) return BadRequest();
            _service.Update(proprietaire);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User proprietaire)
        {
            var result = _service.Login(proprietaire.email, proprietaire.password);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
    }
}
