using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Pets.Commands.ChooseGeneralPhoto;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record ChooseGeneralPhotoRequest(string FilePath)
{
    public ChooseGeneralPhotoCommand ToCommand(Guid volunteerId, Guid petId) =>
        new ChooseGeneralPhotoCommand(volunteerId, petId, FilePath);
}