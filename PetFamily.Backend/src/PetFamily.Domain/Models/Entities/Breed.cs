using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Domain.Models.Entities;

public class Breed : Entity<BreedId>
{
    //EF
    private Breed()
    {
    }

    public Breed(BreedId breedId, string breedName)
    {
        Id = breedId;
        BreedName = breedName;
        //SpecieId = specieId;
    }
    public string BreedName { get; private set; } = null!;
    // public SpecieId SpecieId { get; private set; }
    public Species Specie {get; private set;}
}
