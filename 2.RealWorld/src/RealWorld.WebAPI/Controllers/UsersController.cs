using Microsoft.AspNetCore.Mvc;
using RealWorld.WebAPI.Services;

namespace RealWorld.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellation)
    {
        var result = await userService.GetAllAsync(cancellation);
        return Ok(result);
    }
}
