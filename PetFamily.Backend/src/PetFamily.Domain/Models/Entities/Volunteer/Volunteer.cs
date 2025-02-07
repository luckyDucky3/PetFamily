using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities.Volunteer;

public sealed class Volunteer : Entity<VolunteerId>
{
    //EF
    private Volunteer(VolunteerId id) : base(id) {}

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

    private readonly List<RequisitesForHelp> _requisitesForHelp = [];
    public IReadOnlyList<RequisitesForHelp> RequisitesForHelp => _requisitesForHelp;

    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;

    public static Result<Volunteer, Error> Create(
        VolunteerId id, FullName fullName, EmailAddress emailAddress,
        string description, PhoneNumber phoneNumber, int experienceYears)
    {
        Volunteer volunteer = new Volunteer(
            id, fullName, experienceYears, phoneNumber, description, emailAddress);

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
    
    public static Result<int, Error> ExperienceYearsValidation(int years)
    {
        if (years < 0 || years > Constants.MAX_EXP_YEARS)
            return Result.Failure<int, Error>(Errors.General.IsInvalid("Experience years"));
        return Result.Success<int, Error>(years);
    }

    public static Result<string, Error> DescriptionValidation(string description)
    {
        if (description.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Result.Failure<string, Error>(Errors.General.IsInvalidLength("Description"));
        return Result.Success<string, Error>(description);
    }
}