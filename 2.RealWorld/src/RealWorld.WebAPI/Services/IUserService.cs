using RealWorld.WebAPI.DTOs;
using RealWorld.WebAPI.Models;

namespace RealWorld.WebAPI.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync(CancellationToken cancellation = default);
    Task<bool> CreateAsync(CreateUserDto request, CancellationToken cancellation = default);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellation = default);
}
