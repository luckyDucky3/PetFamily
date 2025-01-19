using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.ValueObjects;

namespace PetFamily.Domain.Models.Entities;

public class Volunteer : Entity<VolunteerId>
{
    //EF
    private Volunteer()
    {
    }

    public string Name { get; private set; } = null!;
    public EmailAdress Email { get; private set; }
    public string Description { get; private set; }
    public int ExperienceYears { get; private set; }
    public int CountOfPetsThatFindHome => Pets.Count(p => p.Status == Status.FindHome);
    public int CountOfPetsThatSearchHome => Pets.Count(p => p.Status == Status.SearchHome);
    public int CountOfPetsThatSick => Pets.Count(p => p.Status == Status.Sick);
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    
    public SocialNetworksDetails SocialNetworksDetails { get; private set; }
    public RequisitesForHelpDetails RequisitesForHelpDetails { get; private set; }
    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;
    
}