using Microsoft.AspNetCore.Mvc;
using FotoKlubasSvetaine.Server.Data;
using System.Linq;
using System.Threading.Tasks;
using FotoKlubasSvetaine.Server.Models;
using Microsoft.EntityFrameworkCore;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("userinfo")]
    public async Task<IActionResult> GetUserInfo(string username)
    {
        var user = await _context.Narys.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount(Narys newNarys)
    {
        _context.Narys.Add(newNarys);
        await _context.SaveChangesAsync();
        return Ok("Account created successfully.");
    }

}
