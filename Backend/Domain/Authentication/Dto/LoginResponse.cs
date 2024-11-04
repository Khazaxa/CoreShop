using Domain.Users.Enums;

namespace Domain.Authentication.Dto;

public record LoginResponseDto(
    int Id,
    string Email,
    string Name,
    string Surname,
    UserRole Role,
    string AccessToken
);