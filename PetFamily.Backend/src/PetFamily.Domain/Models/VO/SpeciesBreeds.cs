using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class SpeciesBreeds : ValueObject
{
    public SpecieId SpeciesId { get; }
    public BreedId BreedId { get; }

    public SpeciesBreeds(SpecieId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
    public static Result<SpeciesBreeds, Error> Create(SpecieId speciesId, BreedId breedId)
    {
        if (speciesId.Value == Guid.Empty || breedId.Value == Guid.Empty)
            return Result.Failure<SpeciesBreeds, Error>(
                Errors.General.IsRequired("Species and Breed ids"));
        var resp = new SpeciesBreeds(speciesId, breedId);
        return Result.Success<SpeciesBreeds, Error>(resp);
    }
}