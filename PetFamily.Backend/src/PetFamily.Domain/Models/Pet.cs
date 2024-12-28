using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.ValueObjects;

namespace PetFamily.Domain.Models;

public class Pet : Entity<Guid>
{
    //EF
    private Pet()
    {
        
    }

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public SpeciesBreeds SpeciesBreeds { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string InfoAboutHealth { get; private set; } = null!;
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public string PhoneNumber { get; private set; } = null!;
    public bool IsCastrate { get; private set; }
    public bool IsVaccinate { get; private set; }
    public DateTime BirthDate { get; private set; } = default!;
    public Status Status { get; private set; }
    public DateTime CreatedOn { get; private set; } = default!;
}