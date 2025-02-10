using FotoKlubasSvetaine.Server.Models;
using FotoKlubasSvetaine.Server.Repositories;

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

            // Add a new Fotografija
            endpoints.MapPost("/fotografija", async (Fotografija fotografija, IFotografijaRepository repository) =>
            {
                await repository.AddFotografija(fotografija);
                return Results.Created($"/fotografija/{fotografija.FotoID}", fotografija);
            })
            .WithTags("Fotografija")
            .WithName("CreateFotografija");

            // Update an existing Fotografija
            endpoints.MapPut("/fotografija/{id:int}", async (int id, Fotografija fotografija, IFotografijaRepository repository) =>
            {
                if (id != fotografija.FotoID)
                {
                    return Results.BadRequest();
                }
                await repository.UpdateFotografija(fotografija);
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
        }
    }
}
