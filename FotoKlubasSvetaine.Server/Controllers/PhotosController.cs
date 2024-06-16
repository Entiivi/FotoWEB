using Microsoft.AspNetCore.Mvc;
using FotoKlubasWebApp.Models;
using System.Collections.Generic;

namespace FotoKlubasWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotosController : ControllerBase
    {
        private static List<Photo> photos = new List<Photo>
        {
            new Photo { Id = 1, Title = "Photo 1", Description = "Description 1", ImageUrl = "url1.jpg" },
            new Photo { Id = 2, Title = "Photo 2", Description = "Description 2", ImageUrl = "url2.jpg" }
        };

        [HttpGet]
        public IEnumerable<Photo> Get()
        {
            return photos;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Photo photo)
        {
            photo.Id = photos.Count + 1;
            photos.Add(photo);
            return CreatedAtAction(nameof(Get), new { id = photo.Id }, photo);
        }
    }
}
