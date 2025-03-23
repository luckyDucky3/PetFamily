using PetFamily.Application.Volunteers.Pets.Commands.RemovePet;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record RemovePetRequest(string FilePath, string BucketName)
{
    public RemovePetQuery ToQuery() => new RemovePetQuery(FilePath, BucketName);
}