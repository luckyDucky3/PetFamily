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

    public FullName Name { get; private set; }
    public EmailAddress? Email { get; private set; }
    public string? Description { get; private set; }
    public int ExperienceYears { get; private set; }
    public int CountOfPetsThatFindHome => Pets.Count(p => p.Status == Status.FindHome);
    public int CountOfPetsThatSearchHome => Pets.Count(p => p.Status == Status.SearchHome);
    public int CountOfPetsThatSick => Pets.Count(p => p.Status == Status.Sick);
    public PhoneNumber? PhoneNumber { get; private set; }
    
    public SocialNetworksDetails? SocialNetworksDetails { get; private set; }
    public RequisitesForHelpDetails? RequisitesForHelpDetails { get; private set; }
    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;
    
    public static Result<Volunteer, Error> Create(VolunteerId id, FullName fullName, EmailAddress? emailAdress = null, 
        string? description = null, PhoneNumber? phoneNumber = null, int experienceYears = 0)
    {
        Volunteer volunteer = new Volunteer(id, fullName, experienceYears, phoneNumber,
            description, emailAdress);
        return Result.Success<Volunteer, Error>(volunteer);
    }
    private Volunteer(VolunteerId id, FullName fullName, int experienceYears)
    {
        Id = id;
        Name = fullName;
        ExperienceYears = experienceYears;
    }
    private Volunteer(VolunteerId id, FullName fullName, int experienceYears, PhoneNumber? phoneNumber,
        string? description, EmailAddress? emailAdress) : this(id, fullName, experienceYears)
    {
        Email = emailAdress;
        Description = description;
        PhoneNumber = phoneNumber;
    }
}