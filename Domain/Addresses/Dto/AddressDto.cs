namespace Domain.Addresses.Dto;

public record AddressDto(
    int Id,
    string? Street,
    int Number,
    int? Apartment,
    string City,
    string PostalCode,
    string Country,
    bool IsMain
    );