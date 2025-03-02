using PetFamily.Application.Volunteers._Dto;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record UpdateMainInfoRequest(
    FullNameDto Fullname,
    string Description,
    string EmailAddress,
    string PhoneNumber,
    int ExperienceYears)
{
    public UpdateMainInfoCommand ToCommand(Guid id)
        => new UpdateMainInfoCommand(id, Fullname, Description, EmailAddress, PhoneNumber, ExperienceYears);
};