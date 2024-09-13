using System.ComponentModel.DataAnnotations;
using Domain.Users.Enums;

namespace Domain.Users.Dto;

public record UserParams(
    [Required]
    string Name,
    [Required]
    string Surname,
    [Required]
    string Phone,
    [Required]
    string Email,
    [Required]
    string Password,
    [Required]
    UserRole Role
    );