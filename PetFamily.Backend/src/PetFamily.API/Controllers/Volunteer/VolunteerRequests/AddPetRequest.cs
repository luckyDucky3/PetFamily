using PetFamily.Application.Volunteers.Pets.Commands.AddPet;
using PetFamily.Application.Volunteers.Pets.Commands.PetDtos;
using PetFamily.Domain.Enums;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record AddPetRequest(
    string PetName,
    Guid SpecieId,
    Guid BreedId,
    Color Color,
    AddressDto AddressDto)
{
    public AddPetCommand ToCommand(Guid id)
        => new AddPetCommand(id, PetName, SpecieId, BreedId, Color, AddressDto);
};


    