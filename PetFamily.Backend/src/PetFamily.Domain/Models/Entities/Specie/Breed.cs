using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities.Specie;

public sealed class Breed : Entity<BreedId>
{
    //EF
    private Breed(BreedId id) : base(id) {}

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
            return Result.Failure<Breed, Error>(Errors.General.IsRequired("Breed name"));
        Breed breed = new Breed(breedId, breedName);
        return Result.Success<Breed, Error>(breed);
    }
}
