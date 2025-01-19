using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Domain.Models.Entities;

public class Pet : Entity<PetId>
{
    //EF
    private Pet()
    {
        
    }

    public Pet(string name, string description, SpeciesBreeds speciesBreeds, string color, string address)
    {
        Name = name;
        Description = description;
        SpeciesBreeds = speciesBreeds;
        Color = color;
        Address = address;
    }
    
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public SpeciesBreeds SpeciesBreeds { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string InfoAboutHealth { get; private set; } = null!;
    public double? Weight { get; private set; }
    public double? Height { get; private set; }
    public string PhoneNumber { get; private set; } = null!;
    public bool IsCastrate { get; private set; }
    public bool IsVaccinate { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedOn { get; private set; } = default!;
}