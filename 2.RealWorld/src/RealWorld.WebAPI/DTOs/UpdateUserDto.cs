namespace RealWorld.WebAPI.DTOs;

public sealed record UpdateUserDto(
    int id,
    string Name,
    int Age,
    DateOnly DateOfBirth);
