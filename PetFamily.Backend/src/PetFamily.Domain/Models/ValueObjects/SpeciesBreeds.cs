using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Domain.Models.ValueObjects;

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
}