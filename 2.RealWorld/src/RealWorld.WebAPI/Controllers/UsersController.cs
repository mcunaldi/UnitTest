using Microsoft.AspNetCore.Mvc;
using RealWorld.WebAPI.DTOs;
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

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto request, CancellationToken cancellation)
    {
        var result = await userService.CreateAsync(request, cancellation);
        if (result)
        {
            return Ok(new { Message = "Kullanıcı kaydı başarılı" });
        }


        return BadRequest(new { Message = "Kullanıcı kaydı sırasında bir hatayla karşılaşıldı." });
    }

    [HttpGet]
    public async Task<IActionResult> DeleteById(int id, CancellationToken cancellationToken)
    {
        var result = await userService.DeleteByIdAsync(id, cancellationToken);
        if (result)
        {
            return Ok(new { Message = "Kullanıcı başarıyla silindi" });
        }

        return BadRequest(new { Message = "Kullanıcı silerken bir hatayla karşılaştık." });
    }


    [HttpPost]
    public async Task<IActionResult> UpdateAsync(UpdateUserDto request, CancellationToken cancellation)
    {
        var result = await userService.UpdateAsync(request, cancellation);
        if (result)
        {
            return Ok(new { Message = "Kullanıcı güncelleme başarılı" });
        }


        return BadRequest(new { Message = "Kullanıcı güncelleme sırasında bir hatayla karşılaşıldı." });
    }
}
