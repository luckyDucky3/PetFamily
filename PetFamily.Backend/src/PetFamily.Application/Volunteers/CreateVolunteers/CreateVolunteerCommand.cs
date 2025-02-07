using PetFamily.Application.Volunteers.Dto;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.Volunteers.CreateVolunteers;

public record CreateVolunteerCommand
{
    public FullNameDto FullName { get; }
    public string Description { get; }
    public string PhoneNumber { get; }
    public string EmailAddress { get; }
    public int ExperienceYears { get; }
    public List<SocialNetworkDto> SocialNetworks { get; }
    public List<RequisitesForHelpDto> RequisitesForHelp { get; }

    public CreateVolunteerCommand(CreateVolunteerRequest createVolunteerRequest)
    {
        FullName = createVolunteerRequest.FullName;
        Description = createVolunteerRequest.Description;
        PhoneNumber = createVolunteerRequest.PhoneNumber;
        EmailAddress = createVolunteerRequest.EmailAddress;
        ExperienceYears = createVolunteerRequest.ExperienceYears;
        SocialNetworks = createVolunteerRequest.SocialNetworks != null
            ? new List<SocialNetworkDto>(createVolunteerRequest.SocialNetworks)
            : new List<SocialNetworkDto>();
        RequisitesForHelp = createVolunteerRequest.RequisitesForHelp != null
            ? new List<RequisitesForHelpDto>(createVolunteerRequest.RequisitesForHelp)
            : new List<RequisitesForHelpDto>();
    }
}