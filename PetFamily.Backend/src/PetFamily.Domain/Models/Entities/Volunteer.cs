using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities;

public sealed class Volunteer : Entity<VolunteerId>
{
    //EF
    private Volunteer()
    {
    }

    public FullName Name { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int ExperienceYears { get; private set; }
    public int CountOfPetsThatFindHome => Pets.Count(p => p.Status == Status.FindHome);
    public int CountOfPetsThatSearchHome => Pets.Count(p => p.Status == Status.SearchHome);
    public int CountOfPetsThatSick => Pets.Count(p => p.Status == Status.Sick);
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    
    //public SocialNetworksDetails? SocialNetworksDetails { get; private set; }
    //public RequisitesForHelpDetails? RequisitesForHelpDetails { get; private set; }
    private readonly List<SocialNetwork> _socialNetworks = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    
    private readonly List<RequisitesForHelp> _requisitesForHelp = [];
    public IReadOnlyList<RequisitesForHelp> RequisitesForHelp => _requisitesForHelp;
    
    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;
    
    public static Result<Volunteer, Error> Create(
        VolunteerId id, FullName fullName, EmailAddress emailAdress, 
        string description, PhoneNumber phoneNumber, int experienceYears)
    {
        Volunteer volunteer = new Volunteer(
            id, fullName, experienceYears, phoneNumber, description, emailAdress);
        
        return Result.Success<Volunteer, Error>(volunteer);
    }
    private Volunteer(
        VolunteerId id, FullName fullName, int experienceYears, 
        PhoneNumber phoneNumber, string description, EmailAddress emailAddress)
    {
        Id = id;
        Name = fullName;
        ExperienceYears = experienceYears;
        Email = emailAddress;
        Description = description;
        PhoneNumber = phoneNumber;
    }

    public void CreateSocialNetworks(List<SocialNetwork> socialNetworks) 
        => _socialNetworks.AddRange(socialNetworks);

    public void CreateRequisitesForHelp(List<RequisitesForHelp> requisitesForHelp) 
        => _requisitesForHelp.AddRange(requisitesForHelp);
}