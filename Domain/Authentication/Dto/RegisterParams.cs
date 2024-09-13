namespace Domain.Authentication.Dto;

public record RegisterParams(
    string Name,
    string Surname,
    string Phone,
    string Email,
    string Password
    );