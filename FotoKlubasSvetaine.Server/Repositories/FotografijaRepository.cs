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
            return await _context.Fotografija.ToListAsync();
        }

        public async Task<Fotografija> GetFotografija(int id)
        {
            return await _context.Fotografija.FindAsync(id);
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
            var fotografija = await _context.Fotografija.FindAsync(id);
            _context.Fotografija.Remove(fotografija);
            await _context.SaveChangesAsync();
        }
    }
}
