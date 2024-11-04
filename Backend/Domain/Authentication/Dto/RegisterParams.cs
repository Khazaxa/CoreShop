namespace Domain.Authentication.Dto;

public record RegisterParams(
    string Name,
    string Surname,
    string AreaCode,
    string Phone,
    string Email,
    string Password
    );