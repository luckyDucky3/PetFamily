using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.ValueObjects;

namespace PetFamily.Domain.Models;

public class Volunteer : Entity<Guid>
{
    //EF
    private Volunteer()
    {
    }

    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int ExperienceYears { get; private set; }
    public int CountOfPetsThatFindHome => Pets.Count(x => x.Status == Status.FindHome);
    public int CountOfPetsThatSearchHome => Pets.Count(x => x.Status == Status.SearchHome);
    public int CountOfPetsThatSick => Pets.Count(x => x.Status == Status.Sick);
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    private readonly List<SocialNetwork> _socialNetworks = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    private readonly List<RequisitesForHelp> _requisitesForHelp = [];
    public IReadOnlyList<RequisitesForHelp> RequisitesForHelp => _requisitesForHelp;
    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;
}