using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities.Volunteer;

public sealed class Volunteer : Entity<VolunteerId>, ISoftDeletable
{
    public bool IsDeleted { get; private set; } = false;
    //EF
    private Volunteer(VolunteerId id) : base(id)
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

    private readonly List<SocialNetwork> _socialNetworks = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    private readonly List<HelpRequisite> _helpRequisites = [];
    public IReadOnlyList<HelpRequisite> HelpRequisites => _helpRequisites;

    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;

    public static Result<Volunteer, Error> Create(
        VolunteerId id, 
        FullName fullName, 
        string description, 
        EmailAddress emailAddress, 
        PhoneNumber phoneNumber, 
        int experienceYears)
    {
        Volunteer volunteer = new Volunteer(
            id, fullName, description, emailAddress, phoneNumber, experienceYears);

        return Result.Success<Volunteer, Error>(volunteer);
    }

    private Volunteer(
        VolunteerId id,
        FullName fullName, 
        string description, 
        EmailAddress emailAddress, 
        PhoneNumber phoneNumber, 
        int experienceYears)
    {
        Id = id;
        Name = fullName;
        ExperienceYears = experienceYears;
        Email = emailAddress;
        Description = description;
        PhoneNumber = phoneNumber;
    }

    public void AddSocialNetworks(List<SocialNetwork> socialNetworks)
        => _socialNetworks.AddRange(socialNetworks);

    public void AddHelpRequisites(List<HelpRequisite> helpRequisites)
        => _helpRequisites.AddRange(helpRequisites);
    
    public void UpdateSocialNetworks(List<SocialNetwork> socialNetworks)
    {
        _socialNetworks.Clear();
        _socialNetworks.AddRange(socialNetworks);
    }
    
    public void UpdateHelpRequisites(List<HelpRequisite> helpRequisites)
    {
        _helpRequisites.Clear();
        _helpRequisites.AddRange(helpRequisites);
    }
    
    public void UpdateMainInfo(
        FullName fullName, 
        string description, 
        EmailAddress emailAddress, 
        PhoneNumber phoneNumber, 
        int experienceYears)
    {
        Name = fullName;
        Description = description;
        Email = emailAddress;
        PhoneNumber = phoneNumber;
        ExperienceYears = experienceYears;
    }

    public void Deactivate()
    {
        if (!IsDeleted)
            IsDeleted = true;

        foreach (var pet in Pets)
        {
            pet.Deactivate();
        }
    }

    public void Activate()
    {
        if (IsDeleted)
            IsDeleted = false;
    }
    
}