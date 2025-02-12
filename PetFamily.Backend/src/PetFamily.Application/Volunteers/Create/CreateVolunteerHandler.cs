using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    string EmailAddress,
    int ExperienceYears,
    List<SocialNetworkDto>? SocialNetworks,
    List<HelpRequisiteDto>? RequisitesForHelp);
    
public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    string PhoneNumber,
    string EmailAddress,
    int ExperienceYears,
    List<SocialNetworkDto>? SocialNetworks,
    List<HelpRequisiteDto>? RequisitesForHelp);

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository, 
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerCommand createVolunteerCommand, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var experienceYears = createVolunteerCommand.ExperienceYears;

        var description = createVolunteerCommand.Description;

        var fullNameResult = FullName.Create(
            createVolunteerCommand.FullName.FirstName,
            createVolunteerCommand.FullName.LastName,
            createVolunteerCommand.FullName.Patronymic).Value;

        var phoneNumber = PhoneNumber.Create(
            createVolunteerCommand.PhoneNumber).Value;

        var emailAddress = EmailAddress.Create(
            createVolunteerCommand.EmailAddress).Value;

        List<SocialNetwork> socialNetworks = [];
        if (createVolunteerCommand.SocialNetworks is not null)
            socialNetworks.AddRange(
                createVolunteerCommand.SocialNetworks.Select(
                    socialNetwork => SocialNetwork.Create(
                        socialNetwork.Name, socialNetwork.Link).Value));

        
        List<HelpRequisite> requisitesForHelp = [];
        if (createVolunteerCommand.RequisitesForHelp is not null)
            requisitesForHelp.AddRange(
                createVolunteerCommand.RequisitesForHelp.Select(
                    requisiteForHelp => HelpRequisite.Create(
                        requisiteForHelp.Title, requisiteForHelp.Description).Value));

        var volunteerResult = Volunteer.Create(
            volunteerId, 
            fullNameResult,
            description,
            emailAddress,
            phoneNumber, 
            experienceYears);

        if (volunteerResult.IsFailure)
            return Result.Failure<Guid, Error>(volunteerResult.Error);

        if (socialNetworks.Any())
            volunteerResult.Value.AddSocialNetworks(socialNetworks);

        if (requisitesForHelp.Any())
            volunteerResult.Value.AddHelpRequisites(requisitesForHelp);

        Guid vId = await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("Volunteer has been successfully added");
        
        return Result.Success<Guid, Error>(vId);
    }
}