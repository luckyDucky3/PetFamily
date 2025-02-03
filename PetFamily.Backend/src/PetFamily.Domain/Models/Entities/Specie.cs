using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities;

public sealed class Specie : Entity<SpecieId>
{
    private readonly List<Breed> _breeds = [];
    //EF
    private Specie()
    {
    }

    private Specie(SpecieId specieId, string specieName)
    {
        Id = specieId;
        SpecieName = specieName;
    }
    
    public string SpecieName { get; private set; } = null!;
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Specie, Error> Create(SpecieId specieId, string specieName)
    {
        if (string.IsNullOrWhiteSpace(specieName))
            return Result.Failure<Specie, Error>(Errors.General.IsNullOrWhitespace("Specie name"));
        
        Specie specie = new Specie(specieId, specieName);
        return Result.Success<Specie, Error>(specie);
    }
}