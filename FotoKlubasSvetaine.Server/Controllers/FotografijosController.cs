namespace FotoKlubasSvetaine.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FotoKlubasSvetaine.Server.Models;
    using FotoKlubasSvetaine.Server.Repositories;

    [Route("fotografija")]
    [ApiController]
    public class FotografijosController : ControllerBase
    {
        private readonly IFotografijaRepository _repository;

        public FotografijosController(IFotografijaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fotografija>>> GetFotografijos()
        {
            return Ok(await _repository.GetFotografijos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fotografija>> GetFotografija(int id)
        {
            var fotografija = await _repository.GetFotografija(id);
            if (fotografija == null)
            {
                return NotFound();
            }
            return fotografija;
        }

        [HttpPost]
        public async Task<ActionResult<Fotografija>> PostFotografija(Fotografija fotografija)
        {
            await _repository.AddFotografija(fotografija);
            return CreatedAtAction(nameof(GetFotografija), new { id = fotografija.FotoID }, fotografija);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFotografija(int id, Fotografija fotografija)
        {
            if (id != fotografija.FotoID)
            {
                return BadRequest();
            }
            await _repository.UpdateFotografija(fotografija);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFotografija(int id)
        {
            await _repository.DeleteFotografija(id);
            return NoContent();
        }
    }
}
