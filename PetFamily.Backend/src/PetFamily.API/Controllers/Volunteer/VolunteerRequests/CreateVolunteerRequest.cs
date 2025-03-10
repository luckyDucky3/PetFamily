using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Commands.Create;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    string EmailAddress,
    int ExperienceYears,
    List<SocialNetworkDto>? SocialNetworks,
    List<HelpRequisiteDto>? RequisitesForHelp)
{
    public CreateVolunteerCommand ToCommand()
        => new CreateVolunteerCommand(
            FullName,
            Description,
            PhoneNumber,
            EmailAddress,
            ExperienceYears, 
            SocialNetworks,
            RequisitesForHelp);
};