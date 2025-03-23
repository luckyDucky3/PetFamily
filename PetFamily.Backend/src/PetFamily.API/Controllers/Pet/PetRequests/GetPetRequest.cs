using PetFamily.Application.Volunteers.Pets.Queries.GetFilePet;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record GetPetRequest(string FilePath, string BucketName)
{
    public GetPetQuery ToQuery() => new GetPetQuery(FilePath, BucketName);
}