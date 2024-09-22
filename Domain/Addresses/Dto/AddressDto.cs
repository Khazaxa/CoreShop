namespace Domain.Addresses.Dto;

public record AddressDto(
    string? Street,
    string Number,
    string? Apartment,
    string City,
    string PostalCode,
    string Country,
    bool IsMain
);