using PetFamily.Application.Volunteers.Pets.Commands.RemoveFilePet;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record RemoveFileRequest(string FilePath, string BucketName)
{
    public RemoveFilePetQuery ToQuery() => new RemoveFilePetQuery(FilePath, BucketName);
}