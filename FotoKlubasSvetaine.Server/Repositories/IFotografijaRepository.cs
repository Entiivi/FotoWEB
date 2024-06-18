namespace FotoKlubasSvetaine.Server.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FotoKlubasSvetaine.Server.Models;

    public interface IFotografijaRepository
    {
        Task<IEnumerable<Fotografija>> GetFotografijos();
        Task<Fotografija> GetFotografija(int id);
        Task AddFotografija(Fotografija fotografija);
        Task UpdateFotografija(Fotografija fotografija);
        Task DeleteFotografija(int id);
    }
}
