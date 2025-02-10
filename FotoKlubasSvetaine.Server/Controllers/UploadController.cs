using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

public static class UploadEndpoints
{
    public static void MapUploadEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/upload", async (
            [FromForm] IFormFile photo,
            [FromForm] string pavadinimas,
            [FromForm] string aprasymas,
            [FromForm] int narysID,
            [FromForm] int klubasID,
            IWebHostEnvironment env,
            ApplicationDbContext context,
            [FromServices] IAntiforgery antiforgery,
            HttpContext httpContext) =>
        {
            // Validate the anti-forgery token
            await antiforgery.ValidateRequestAsync(httpContext);

            if (photo == null || photo.Length == 0)
            {
                return Results.BadRequest(new { Message = "No file uploaded." });
            }

            if (!photo.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest(new { Message = "Only .jpg files are allowed." });
            }

            var uploadsFolderPath = Path.Combine(env.ContentRootPath, "Nuotraukos");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            var fotografija = new Fotografija
            {
                Pavadinimas = pavadinimas,
                Aprasymas = aprasymas,
                NarysID = narysID,
                KlubasID = klubasID,
                FotoPath = $"Nuotraukos/{fileName}",
                Data = DateTime.UtcNow
            };

            context.Fotografija.Add(fotografija);
            await context.SaveChangesAsync();

            return Results.Ok(new { Message = "Photo uploaded successfully.", Path = fotografija.FotoPath });
        });
    }
}
