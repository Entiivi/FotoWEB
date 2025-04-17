namespace FotoKlubasSvetaine.Server.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using FotoKlubasSvetaine.Server.Data;
    using FotoKlubasSvetaine.Server.Models;

    public class FotografijaRepository : IFotografijaRepository
    {
        private readonly ApplicationDbContext _context;

        public FotografijaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fotografija>> GetFotografijos()
        {
            return await _context.Fotografija
               .Include(f => f.Narys)
               .Include(f => f.Klubas)
               .ToListAsync();
        }

        public async Task<Fotografija> GetFotografija(int id)
        {
            // Explicitly filter on FotoID
            return await _context.Fotografija
                                 .FirstOrDefaultAsync(f => f.FotoID == id);
        }

        public async Task AddFotografija(Fotografija fotografija)
        {
            _context.Fotografija.Add(fotografija);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFotografija(Fotografija fotografija)
        {
            _context.Entry(fotografija).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFotografija(int id)
        {
            var foto = await GetFotografija(id);
            if (foto != null)
            {
                _context.Fotografija.Remove(foto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Fotografija?> GetFotografijaByPavadinimas(string pavadinimas)
        {
            return await _context.Fotografija
                .FirstOrDefaultAsync(f => f.Pavadinimas == pavadinimas);
        }

        public async Task<IEnumerable<object>> GetFotoInfoForChatbot()
        {
            return await _context.Fotografija
                .Include(f => f.Narys)
                .Include(f => f.Klubas)
                .Select(f => new
                {
                    f.FotoID,
                    f.Pavadinimas,
                    f.Aprasymas,
                    f.Data,
                    Narys = new
                    {
                        f.Narys.Vardas,
                        f.Narys.Pavarde
                    },
                    Klubas = new
                    {
                        f.Klubas.Pavadinimas
                    },
                    f.FotoPath
                })
                .ToListAsync();
        }

    }
}
