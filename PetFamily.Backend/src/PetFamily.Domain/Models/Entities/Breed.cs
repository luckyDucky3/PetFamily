using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities;

public sealed class Breed : Entity<BreedId>
{
    //EF
    private Breed()
    {
    }

    private Breed(BreedId breedId, string breedName)
    {
        Id = breedId;
        BreedName = breedName;
    }
    public string BreedName { get; private set; } = null!;
    public Specie Specie {get; private set;} = null!;

    public static Result<Breed, Error> Create(BreedId breedId, string breedName)
    {
        if (string.IsNullOrWhiteSpace(breedName))
            return Result.Failure<Breed, Error>(Errors.General.IsNullOrWhitespace("Breed name"));
        Breed breed = new Breed(breedId, breedName);
        return Result.Success<Breed, Error>(breed);
    }
}
