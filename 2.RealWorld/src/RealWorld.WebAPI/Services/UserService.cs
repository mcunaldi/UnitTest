using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;

namespace RealWorld.WebAPI.Services;

public sealed class UserService(
    IUserRepository userRepository, ILoggerAdaptor<UserService> logger) : IUserService
{
    public async Task<List<User>> GetAllAsync(CancellationToken cancellation = default)
    {
        logger.LogInformation("Tüm userlar getiriliyor.");
		try
		{
			return await userRepository.GetAllAsync(cancellation);
		}
		catch (Exception ex)
		{

			logger.LogError(ex, "User listesini çekerken bir hatayla karşılaştık");
			throw;
		}
		finally
		{
			logger.LogInformation("Tüm user listesi çekildi");
		}
    }
}
