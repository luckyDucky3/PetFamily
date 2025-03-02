namespace PetFamily.Application.Volunteers.Pets.PetDtos;

public record AddressDto(
    string Country,
    string City,
    string Street,
    string HomeNumber);