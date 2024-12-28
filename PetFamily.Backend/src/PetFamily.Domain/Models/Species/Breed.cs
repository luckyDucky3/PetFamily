using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Species;

public class Breed : Entity<Guid>
{
    //EF
    private Breed()
    {
    }
    public string BreedName { get; private set; } = null!;
    private readonly List<Breed> _breeds = [];
    public IReadOnlyList<Breed> Breeds => _breeds;
}