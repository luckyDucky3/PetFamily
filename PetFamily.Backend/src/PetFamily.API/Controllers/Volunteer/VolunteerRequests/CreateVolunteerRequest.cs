using PetFamily.Application.Volunteers._Dto;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    string EmailAddress,
    int ExperienceYears,
    List<SocialNetworkDto>? SocialNetworks,
    List<HelpRequisiteDto>? RequisitesForHelp);