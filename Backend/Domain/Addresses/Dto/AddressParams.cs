namespace Domain.Addresses.Dto;

public record AddressParams(
    string? Street,
    string Number,
    string? Apartment,
    string City,
    string PostalCode,
    string Country
);