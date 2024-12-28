using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Species;

public class Species : Entity<Guid>
{
    //EF
    private Species()
    {
    }
    
    public string BreedName { get; private set; } = null!;
}