using Domain.Users.Enums;

namespace Domain.Users.Dto;

public record UserDto(
    int Id,
    string Name,
    string Surname,
    string AreaCode,
    string Phone,
    string Email,
    UserRole Role,
    List<int> AddressIds
    );