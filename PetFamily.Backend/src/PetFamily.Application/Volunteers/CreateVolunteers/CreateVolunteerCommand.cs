using PetFamily.Application.Volunteers.DTO;

namespace PetFamily.Application.Volunteers.CreateVolunteers;

public record CreateVolunteerCommand
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? Patronymic { get; }
    public string Description { get; }
    public string PhoneNumber { get; }
    public string EmailAddress { get; } 
    public int ExperienceYears { get; }
    public List<SocialNetworkDto> SocialNetworks { get; }
    public List<RequisitesForHelpDto> RequisitesForHelp { get; }

    public CreateVolunteerCommand(CreateVolunteerRequest createVolunteerRequest)
    {
        FirstName = createVolunteerRequest.FirstName;
        LastName = createVolunteerRequest.LastName;
        Patronymic = createVolunteerRequest.Patronymic;
        Description = createVolunteerRequest.Description;
        PhoneNumber = createVolunteerRequest.PhoneNumber;
        EmailAddress = createVolunteerRequest.EmailAddress;
        ExperienceYears = createVolunteerRequest.ExperienceYears;
        SocialNetworks = createVolunteerRequest.SocialNetworks != null ? 
            new List<SocialNetworkDto>(createVolunteerRequest.SocialNetworks) 
            : new List<SocialNetworkDto>();
        RequisitesForHelp = createVolunteerRequest.RequisitesForHelp != null ?
            new List<RequisitesForHelpDto>(createVolunteerRequest.RequisitesForHelp)
            : new List<RequisitesForHelpDto>();
    }
}