using FotoKlubasSvetaine.Server.Models;
using FotoKlubasSvetaine.Server.Repositories;
using FotoKlubasSvetaine.DTOs;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;


namespace FotoKlubasSvetaine.Server.Controllers
{
    public static class FotografijaEndpoints
    {
        public static void MapFotografijaEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Get all Fotografijos
            endpoints.MapGet("/fotografija", async (IFotografijaRepository repository) =>
            {
                var fotografijos = await repository.GetFotografijos();
                return Results.Ok(fotografijos);
            })
            .WithTags("Fotografija") // Adds a Swagger tag for documentation
            .WithName("GetAllFotografijos"); // Adds an operation ID for Swagger

            // Get a single Fotografija by ID
            endpoints.MapGet("/fotografija/{id:int}", async (int id, IFotografijaRepository repository) =>
            {
                var fotografija = await repository.GetFotografija(id);
                return fotografija != null ? Results.Ok(fotografija) : Results.NotFound();
            })
            .WithTags("Fotografija")
            .WithName("GetFotografijaById");

            endpoints.MapGet("/fotografija/pavadinimas/{pavadinimas}", async (string pavadinimas, IFotografijaRepository repository) =>
            {
                var photo = await repository.GetFotografijaByPavadinimas(pavadinimas);
                return photo != null ? Results.Ok(photo) : Results.NotFound();
            })
            .WithTags("Fotografija")
            .WithName("GetFotografijaByPavadinimas");

            // Add a new Fotografija
            endpoints.MapPost("/fotografija", async (FotografijaDto dto, IFotografijaRepository repository) =>
            {
                var filePath = await SaveBase64Image(dto.FotoData, dto.Pavadinimas);

                var fotografija = new Fotografija
                {
                    Pavadinimas = dto.Pavadinimas,
                    Aprasymas = dto.Aprasymas,
                    Data = dto.Data,
                    NarysID = dto.NarysID,
                    KlubasID = dto.KlubasID,
                    FotoPath = filePath
                };

                await repository.AddFotografija(fotografija);
                return Results.Created($"/fotografija/{fotografija.FotoID}", fotografija);
            })
            .WithMetadata(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute())
            .WithTags("Fotografija")
            .WithName("CreateFotografija");

            // Update an existing Fotografija
            endpoints.MapPut("/fotografija/{id:int}", async (int id, FotografijaDto dto, IFotografijaRepository repository) =>
            {
                var existing = await repository.GetFotografija(id);
                if (existing == null || id != dto.FotoID)
                    return Results.BadRequest();

                var filePath = await SaveBase64Image(dto.FotoData, dto.Pavadinimas);

                existing.Pavadinimas = dto.Pavadinimas;
                existing.Aprasymas = dto.Aprasymas;
                existing.Data = dto.Data;
                existing.NarysID = dto.NarysID;
                existing.KlubasID = dto.KlubasID;
                existing.FotoPath = filePath;

                await repository.UpdateFotografija(existing);
                return Results.NoContent();
            })
            .WithTags("Fotografija")
            .WithName("UpdateFotografija");

            // Delete a Fotografija
            endpoints.MapDelete("/fotografija/{id:int}", async (int id, IFotografijaRepository repository) =>
            {
                var fotografija = await repository.GetFotografija(id);
                if (fotografija == null)
                {
                    return Results.NotFound();
                }
                await repository.DeleteFotografija(id);
                return Results.NoContent();
            })
            .WithTags("Fotografija")
            .WithName("DeleteFotografija");

            // Get all Fotografija info for chatbot
            endpoints.MapGet("/fotografija/info", async (IFotografijaRepository repository) =>
            {
                var enriched = await repository.GetFotoInfoForChatbot();
                return Results.Ok(enriched);
            })
            .WithTags("Fotografija")
            .WithName("GetAllFotoInfo");

        }



        private static async Task<string> SaveBase64Image(string base64, string name)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("FotoData is empty");

            base64 = base64.Replace("data:image/png;base64,", "")
                           .Replace("data:image/jpeg;base64,", "");

            var bytes = Convert.FromBase64String(base64);
            var safeName = name.Replace(" ", "_");
            var fileName = $"{safeName}_{DateTime.Now.Ticks}.jpg";
            var relativePath = Path.Combine("uploads", fileName);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            await File.WriteAllBytesAsync(fullPath, bytes);

            return relativePath.Replace("\\", "/"); // make sure path is web-safe
        }

    }
}
