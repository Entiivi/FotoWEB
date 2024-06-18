using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Models;

namespace FotoKlubasSvetaine.Server.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;

        public LoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Narys> ValidateUserAsync(string username, string password)
        {
            return await _context.Narys
                .FirstOrDefaultAsync(n => n.Username == username && n.Slap == password);
        }
    }
}
