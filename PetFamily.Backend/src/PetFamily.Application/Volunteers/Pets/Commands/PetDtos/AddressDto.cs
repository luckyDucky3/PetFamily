using System.ComponentModel.DataAnnotations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.PetDtos;

public class AddressDto
{
    public string Country { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string HomeNumber { get; init; }
};