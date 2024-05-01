using Microsoft.EntityFrameworkCore;
using RealWorld.WebAPI.Context;
using RealWorld.WebAPI.Models;

namespace RealWorld.WebAPI.Repositories;

public sealed class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<bool> CreateAsync(User user, CancellationToken cancellation = default)
    {
        await context.AddAsync(user, cancellation);
        var result = await context.SaveChangesAsync(cancellation);
        return result > 0;
    }

    public async Task<bool> DeleteByAsync(User user, CancellationToken cancellation = default)
    {
        context.Remove(user);
        var result = await context.SaveChangesAsync(cancellation);
        return result > 0;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users.ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        return await context.Users.FirstOrDefaultAsync(p => p.Id == id, cancellation);
    }

    public async Task<bool> NameIsExist(string name, CancellationToken cancellation = default)
    {
        return await context.Users.AnyAsync(p=> p.Name == name, cancellation);
    }

    public async Task<bool> UpdateByAsync(User user, CancellationToken cancellation = default)
    {
        context.Update(user);
        var result = await context.SaveChangesAsync(cancellation);
        return result > 0;
    }
}
