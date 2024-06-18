using FotoKlubasSvetaine.Server.Models;

public interface ILoginRepository
{
    Task<Narys> ValidateUserAsync(string username, string password);
}
