using Microsoft.AspNetCore.Mvc;
using FotoKlubasSvetaine.Server.Data;
using FotoKlubasSvetaine.Server.Models;
using System.Threading.Tasks;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILoginRepository _loginRepository;

    public LoginController(ApplicationDbContext context, ILoginRepository loginRepository)
    {
        _context = context;
        _loginRepository = loginRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _loginRepository.ValidateUserAsync(model.Username, model.Password);
        if (user != null)
        {
            return Ok(new { Message = "Login successful" });
        }
        return Unauthorized(new { Message = "Invalid credentials" });
    }
}
