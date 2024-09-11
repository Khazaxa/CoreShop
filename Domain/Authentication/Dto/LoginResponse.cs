using Domain.Users.Enums;

namespace Domain.Authentication.Dto;

public record LoginResponseDto(
    int Id,
    string Email,
    string? UserName,
    UserRole? Role,
    string AccessToken
);