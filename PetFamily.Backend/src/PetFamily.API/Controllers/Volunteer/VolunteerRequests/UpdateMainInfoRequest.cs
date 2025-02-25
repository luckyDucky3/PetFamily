using PetFamily.Application.Volunteers._Dto;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record UpdateMainInfoRequest(
    FullNameDto Fullname, 
    string Description, 
    string EmailAddress, 
    string PhoneNumber,
    int ExperienceYears);