using System.ComponentModel.DataAnnotations;
using Domain.Users.Enums;

namespace Domain.Users.Dto;

public record UserParams(
    [Required]
    string UserName,
    [Required]
    string Email,
    [Required]
    string Password,
    [Required]
    UserRole Role
    );