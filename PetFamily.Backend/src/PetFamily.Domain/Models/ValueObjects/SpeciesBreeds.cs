using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class SpeciesBreeds : ValueObject
{
    public Guid SpeciesId { get; }
    public Guid BreedId { get; }

    public SpeciesBreeds(Guid speciesId, Guid breedId)
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