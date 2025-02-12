using PetFamily.Application.Volunteers.Dto;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    string EmailAddress,
    int ExperienceYears,
    List<SocialNetworkDto>? SocialNetworks,
    List<HelpRequisiteDto>? RequisitesForHelp);