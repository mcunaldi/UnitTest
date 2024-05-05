using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using RealWorld.WebAPI.DTOs;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Services;

namespace Users.API.Test.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();   
    private readonly ILoggerAdapter<UserService> logger = Substitute.For<ILoggerAdapter<UserService>>();

    private readonly CreateUserDto createUserDto = new CreateUserDto("Taner Saydam", 34, new DateOnly(1989, 09, 03));

    public UserServiceTests()
    {
        _sut = new(userRepository, logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUserExist()
    {
        //Arrange
        userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        var result = await _sut.GetAllAsync();

        //Asset
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnsUsers_WhenSomeUserExist()
    {
        //Arrange
        var tanerUser = new User
        {
            Id = 1,
            Age = 34,
            Name = "Taner Saydam",
            DateOfBirth = new(1989, 09, 03)
        };

        var mehmetUser = new User
        {
            Id = 1,
            Age = 27,
            Name = "Mehmet Can ÜNALDI",
            DateOfBirth = new(1997, 01, 01)
        };
        var users = new List<User> { tanerUser, mehmetUser };

        userRepository.GetAllAsync().Returns(users);

        //Act

        var result = await _sut.GetAllAsync();

        //Assert
        result.Should().BeEquivalentTo(users);
        result.Should().HaveCount(2);
        result.Should().NotHaveCount(3);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList() );

        //Act

        await _sut.GetAllAsync();

        //Assert

        logger.Received(1).LogInformation(Arg.Is("Tüm userlar getiriliyor."));
        logger.Received(1).LogInformation(Arg.Is("Tüm user listesi çekildi"));
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAnException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("User listesini çekerken bir hatayla karþýlaþtýk");
        userRepository.GetAllAsync().Throws(exception);

        //Act
        var requestAction = async () => await _sut.GetAllAsync();
        
        await requestAction.Should()
            .ThrowAsync<ArgumentException>();



        logger.Received().LogError(Arg.Is(exception), Arg.Is("User listesini çekerken bir hatayla karþýlaþtýk"));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrownAnError_WhenUserCreateDetailIsNotValid()
    {
        //Arrange
        CreateUserDto request = new("", 0, new(2007,01,01));

        //Act
        var action = async () => await _sut.CreateAsync(request);

        //Assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrownAnError_WhenUserNameExist()
    {
        //Arrange
        userRepository.NameIsExist(Arg.Any<string>()).Returns(true);

        //Act
        var action = async () => await _sut.CreateAsync(new("Taner Saydam", 34, new DateOnly(1989, 09, 03)));

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUserDtoToUserObject()
    {

        //Act
        var user = _sut.CreateUserDtoToUserObject(createUserDto);

        //Assert
        user.Name.Should().Be(createUserDto.Name);
        user.Age.Should().Be(createUserDto.Age);
        user.DateOfBirth.Should().Be(createUserDto.DateOfBirth);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDetailsAreValidAndUnique()
    {
        //Arrange
        userRepository.NameIsExist(createUserDto.Name).Returns(false);
        userRepository.CreateAsync(Arg.Any<User>()).Returns(true);

        //Act
        var result = await _sut.CreateAsync(createUserDto);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        userRepository.NameIsExist(createUserDto.Name).Returns(false);
        userRepository.CreateAsync(Arg.Any<User>()).Returns(true);

        //Act
        await _sut.CreateAsync(createUserDto);

        //Assert
        logger.Received(1).LogInformation(
            Arg.Is("Kullanýcý Adý: {0} olan kullanýcý kaydý yapýlmaya baþlandý."),
            Arg.Any<string>());

        logger.Received(1).LogInformation(
            Arg.Is("User ID: {} olan kullanýcý {1} ms'de oluþturuldu."),
            Arg.Any<int>(),
            Arg.Any<long>());

    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("Kullanýcý kaydý esnasýnda bir hatayla karþýlaþýldý.");
        userRepository.CreateAsync(Arg.Any<User>()).Throws(exception);  

        //Act
        var action = async () => await _sut.CreateAsync(createUserDto);

        //Assert

        await action.Should()
            .ThrowAsync<ArgumentException>();

        logger.Received(1).LogError(Arg.Is(exception) ,Arg.Is("Kullanýcý kaydý esnasýnda bir hatayla karþýlaþýldý."));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrownAnError_WhenUserNotExist()
    {
        //Arrange
        int userId = 1;
        userRepository.GetByIdAsync(userId).ReturnsNull();

        //Act

        var action = async () => await _sut.DeleteByIdAsync(userId);

        //Assert

        await action.Should().ThrowAsync<ArgumentException>();

    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExist()
    {
        //Arrange
        int userId = 1;
        User user = new()
        {
            Id = userId,
            Name = "Taner Saydam",
            Age = 34,
            DateOfBirth = new (1989, 09, 03)
        };
        userRepository.GetByIdAsync(userId).Returns(user);
        userRepository.DeleteByAsync(user).Returns(true);

        //Act

        var result = await _sut.DeleteByIdAsync(userId);

        //Assert

        result.Should().BeTrue();

    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        int userId = 1;
        User user = new()
        {
            Id = userId,
            Name = "Taner Saydam",
            Age = 34,
            DateOfBirth = new(1989, 09, 03)
        };
        userRepository.GetByIdAsync(userId).Returns(user);
        userRepository.DeleteByAsync(user).Returns(true);

        //Act

        await _sut.DeleteByIdAsync(userId);

        //Assert

        logger.Received(1).LogInformation(
            Arg.Is("{0} id numarasýna sahip kullanýcý siliniyor..."),
            Arg.Is(userId));

        logger.Received(1).LogInformation(
            Arg.Is("Kullanýcý ID {0} olan kullanýcý kaydý {1} ms de silindi."),
            Arg.Is(userId),
            Arg.Any<long>());

    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        int userId = 1;
        var user = new User()
        {
            Id = userId,
            Name = "Taner Saydam",
            Age = 34,
            DateOfBirth = new(1989, 09, 03)
        };

        userRepository.GetByIdAsync(userId).Returns(user);
        var exception = new ArgumentException("Kullanýcý kaydý silinirken bir hatayla karþýlaþýldý.");

        userRepository.DeleteByAsync(user).Throws(exception);

        //Act

        var action = async () => await _sut.DeleteByIdAsync(userId);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();

        logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Kullanýcý kaydý silinirken bir hatayla karþýlaþýldý."));
    }
}