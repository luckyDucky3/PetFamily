using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class SpeciesBreeds : ValueObject
{
    public Guid SpeciesId { get; }
    public Guid BreedId { get; }

    public SpeciesBreeds(Guid speciesID, Guid breedID)
    {
        SpeciesId = speciesID;
        BreedId = breedID;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesId;
        yield return BreedId;
    }
}