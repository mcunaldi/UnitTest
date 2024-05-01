using FluentAssertions;
using NSubstitute;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Services;

namespace Users.API.Test.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();   
    private readonly ILoggerAdaptor<UserService> logger = Substitute.For<ILoggerAdaptor<UserService>>();

    public UserServiceTests()
    {
        _sut = new(userRepository, logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUserExists()
    {
        //Arrange
        userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        var result = await _sut.GetAllAsync();

        //Asset
        result.Should().BeEmpty();
    }
}