using PetFamily.Application.Volunteers.Dto;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid Id, 
    FullNameDto Fullname, 
    string Description, 
    string EmailAddress,
    string PhoneNumber,
    int ExperienceYears);