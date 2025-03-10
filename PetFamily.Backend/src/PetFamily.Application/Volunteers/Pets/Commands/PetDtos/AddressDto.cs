namespace PetFamily.Application.Volunteers.Pets.Commands.PetDtos;

public record AddressDto(
    string Country,
    string City,
    string Street,
    string HomeNumber);