using FluentValidation.AspNetCore;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.Volunteers.CreateVolunteers;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    string EmailAddress,
    int ExperienceYears,
    List<SocialNetworkDto>? SocialNetworks,
    List<RequisitesForHelpDto>? RequisitesForHelp);