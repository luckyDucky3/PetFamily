using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Domain.Models.Entities;

public class Breed : Entity<BreedId>
{
    //EF
    private Breed()
    {
    }

    public Breed(BreedId breedId, string breedName, SpecieId specieId)
    {
        Id = breedId;
        BreedName = breedName;
        SpeciesId = specieId;
    }
    public string BreedName { get; private set; } = null!;
    public SpecieId SpeciesId { get; private set; }
    public Species Specie {get; private set;}
}
