using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Pets.Commands.UpdatePet;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record UpdateMainInfoPetRequest(PetMainInfoDto MainInfoDto)
{
    public UpdateMainInfoPetCommand ToCommand(Guid volunteerId, Guid petId, PetMainInfoDto petMainInfoDto) =>
        new UpdateMainInfoPetCommand(volunteerId, petId, petMainInfoDto);
}