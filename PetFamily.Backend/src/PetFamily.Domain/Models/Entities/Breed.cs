using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Domain.Models.Entities;

public sealed class Breed : Entity<BreedId>
{
    //EF
    private Breed()
    {
    }

    public Breed(BreedId breedId, string breedName)
    {
        Id = breedId;
        BreedName = breedName;
    }
    public string BreedName { get; private set; } = null!;
    public Species Specie {get; private set;} = null!;
    
}
