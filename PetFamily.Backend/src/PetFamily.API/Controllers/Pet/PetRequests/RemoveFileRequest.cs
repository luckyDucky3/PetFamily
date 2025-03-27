using PetFamily.Application.Volunteers.Pets.Commands.RemoveFilePet;
using PetFamily.Application.Volunteers.Pets.Commands.RemovePet;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record RemoveFileRequest(string FilePath, string BucketName)
{
    public RemoveFilePetQuery ToQuery() => new RemoveFilePetQuery(FilePath, BucketName);
}