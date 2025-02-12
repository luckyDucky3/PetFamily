using PetFamily.Application.Volunteers.Dto;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(Guid Id, UpdateMainInfoDto Dto);

public record UpdateMainInfoDto(
    FullNameDto Fullname, 
    string Description, 
    string EmailAddress, 
    string PhoneNumber,
    int ExperienceYears);