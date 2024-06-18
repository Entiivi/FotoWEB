using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FotoKlubasSvetaine.Server.Controllers
{
    [Route("upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UploadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile photo, [FromForm] string pavadinimas, [FromForm] string aprasymas, [FromForm] int narysID, [FromForm] int klubasID)
        {
            if (photo == null || photo.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await photo.CopyToAsync(memoryStream);
            var fotoData = memoryStream.ToArray();

            var fotografija = new Fotografija
            {
                Pavadinimas = pavadinimas,
                Aprasymas = aprasymas,
                Data = DateTime.UtcNow,
                NarysID = narysID,
                KlubasID = klubasID,
                FotoData = fotoData
            };

            _context.Fotografija.Add(fotografija);
            await _context.SaveChangesAsync();

            return Ok("Photo uploaded successfully.");
        }
    }
}
