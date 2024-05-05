using FluentValidation;
using RealWorld.WebAPI.DTOs;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Validators;
using System.Diagnostics;

namespace RealWorld.WebAPI.Services;

public sealed class UserService(
	IUserRepository userRepository, ILoggerAdapter<UserService> logger) : IUserService
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
	public async Task<bool> CreateAsync(CreateUserDto request, CancellationToken cancellation = default)
	{
		CreateUserDtoValidator validator = new();
		var result = validator.Validate(request);
		if (!result.IsValid)
		{
			throw new ValidationException(string.Join(", ", result.Errors.Select(s => s.ErrorMessage)));
		}

		var nameIsExist = await userRepository.NameIsExist(request.Name, cancellation);
		if (nameIsExist)
		{
			throw new ArgumentException("Bu isim daha önce kaydedilmiş.");
		}

		var user = CreateUserDtoToUserObject(request);

		logger.LogInformation("Kullanıcı Adı: {0} olan kullanıcı kaydı yapılmaya başlandı.", user.Name);

		var stopWatch = Stopwatch.StartNew();
		try
		{

			return await userRepository.CreateAsync(user, cancellation);
		}
		catch (Exception ex)
		{

			logger.LogError(ex, "Kullanıcı kaydı esnasında bir hatayla karşılaşıldı.");
			throw;
		}
		finally
		{
			stopWatch.Stop();
			logger.LogInformation("User ID: {} olan kullanıcı {1} ms'de oluşturuldu.", user.Id, stopWatch.ElapsedMilliseconds);
		}
	}

	public User CreateUserDtoToUserObject(CreateUserDto request)
	{
		return new User()
		{
			Name = request.Name,
			DateOfBirth = request.DateOfBirth,
			Age = request.Age
		};
	}

	public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellation = default)
	{
		User user = await userRepository.GetByIdAsync(id, cancellation);
		if (user is null)
		{
			throw new ArgumentException("Kullanıcı bulunamadı.");
		}

		logger.LogInformation("{0} id numarasına sahip kullanıcı siliniyor...", id);
		var stopWatch = Stopwatch.StartNew();
		try
		{
			return await userRepository.DeleteByAsync(user, cancellation);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Kullanıcı kaydı silinirken bir hatayla karşılaşıldı.");
			throw;
		}
		finally
		{
			stopWatch.Stop();
			logger.LogInformation("Kullanıcı ID {0} olan kullanıcı kaydı {1} ms de silindi.", user.Id, stopWatch.ElapsedMilliseconds);
		}
	}

	public async Task<bool> UpdateAsync(UpdateUserDto request, CancellationToken cancellation = default)
	{
        User? user = await userRepository.GetByIdAsync(request.id);
		if (user is null)
		{
			throw new ArgumentException("Kullanıcı bulunamadı");
		}

        UpdateUserDtoValidator validator = new();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join("\n", result.Errors.Select(s => s.ErrorMessage)));
        }

		if(request.Name == user.Name)
		{
            var nameIsExist = await userRepository.NameIsExist(request.Name, cancellation);
            if (nameIsExist)
            {
                throw new ArgumentException("Bu isim daha önce kaydedilmiş.");
            }
        }

		user.Name = request.Name;
		user.Age = request.Age;
		user.DateOfBirth = request.DateOfBirth;

		//CreateUpdateUserObject(ref user, request); //1

		logger.LogInformation("Kullanıcı Adı: {0} olan kullanıcı kaydı güncellenmeye başlandı.", request.Name);

		var stopWatch = Stopwatch.StartNew();
		try
		{

			return await userRepository.UpdateByAsync(user, cancellation);
		}
		catch (Exception ex)
		{

			logger.LogError(ex, "Kullanıcı güncelleme esnasında bir hatayla karşılaşıldı.");
			throw;
		}
		finally
        {
            stopWatch.Stop();
            logger.LogInformation("Kullanıcı ID {0} olan kullanıcı kaydı {1} ms de güncellendi.", user.Id, stopWatch.ElapsedMilliseconds);
        }
	}

	//private User CreateUpdateUserObject(ref User user, UpdateUserDto request) //1
	//{
	//	user.Name = request.Name;
	//	user.Age = request.Age;
	//	user.DateOfBirth = request.DateOfBirth;

	//	return user;
	//}
}