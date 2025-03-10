using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers._Dto;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Create;

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
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger,
        IValidator<CreateVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand createVolunteerCommand, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(createVolunteerCommand, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
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
            return volunteerResult.Error.ToErrorList();

        if (socialNetworks.Any())
            volunteerResult.Value.AddSocialNetworks(socialNetworks);

        if (requisitesForHelp.Any())
            volunteerResult.Value.AddHelpRequisites(requisitesForHelp);

        Guid id = await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Volunteer has been successfully added");

        return id;
    }
}