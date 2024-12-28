using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class SpeciesBreeds : ValueObject
{
    public Guid SpeciesID { get; }
    public Guid BreedID { get; }

    public SpeciesBreeds(Guid speciesID, Guid breedID)
    {
        SpeciesID = speciesID;
        BreedID = breedID;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SpeciesID;
        yield return BreedID;
    }
}