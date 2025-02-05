using PetFamily.Application.Volunteers.DTO;
using PetFamily.Domain.Models.VO;
namespace PetFamily.Application.Volunteers.CreateVolunteers;


public record CreateVolunteerRequest(
    string FirstName, string LastName, string? Patronymic,
    string Description, string PhoneNumber, string EmailAddress, 
    int ExperienceYears, List<SocialNetworkDto>? SocialNetworks, 
    List<RequisitesForHelpDto>? RequisitesForHelp);


