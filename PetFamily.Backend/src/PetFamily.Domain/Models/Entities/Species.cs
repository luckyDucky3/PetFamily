using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Domain.Models.Entities;

public sealed class Species : Entity<SpecieId>
{
    private readonly List<Breed> _breeds = [];
    //EF
    private Species()
    {
    }

    public Species(SpecieId specieId, string specieName)
    {
        Id = specieId;
        SpecieName = specieName;
    }
    
    public string SpecieName { get; private set; } = null!;
    public IReadOnlyList<Breed> Breeds => _breeds;
}