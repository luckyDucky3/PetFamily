using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;

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