namespace RealWorld.WebAPI.DTOs;

public sealed record CreateUserDto(
    string Name,
    int Age,
    DateOnly DateOfBirth);
