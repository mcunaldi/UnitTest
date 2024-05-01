using RealWorld.WebAPI.Models;

namespace RealWorld.WebAPI.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> NameIsExist(string name, CancellationToken cancellation = default);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<bool> CreateAsync(User user, CancellationToken cancellation = default);
    Task<bool> DeleteByAsync(User user, CancellationToken cancellation = default);
    Task<bool> UpdateByAsync(User user, CancellationToken cancellation = default);
}
