using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.Commands.AddSocialNetworks;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.AddHelpRequisites;

public record AddHelpRequisitesCommand(Guid Id, List<HelpRequisiteDto> HelpRequisiteDtos) : ICommand;

public class AddHelpRequisitesHandler : ICommandHandler<Guid, AddHelpRequisitesCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddSocialNetworksHandler> _logger;
    private readonly IValidator<AddHelpRequisitesCommand> _validator;

    public AddHelpRequisitesHandler(
        IVolunteersRepository volunteersRepository, 
        ILogger<AddSocialNetworksHandler> logger,
        IValidator<AddHelpRequisitesCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddHelpRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(volunteerId.Value).ToErrorList()!;

        List<HelpRequisite> helpRequisites = [];
        helpRequisites.AddRange(command.HelpRequisiteDtos.Select(helpRequisiteDto =>
            HelpRequisite.Create(helpRequisiteDto.Title, helpRequisiteDto.Description).Value));

        volunteer.UpdateHelpRequisites(helpRequisites);
        
        await _volunteersRepository.Save(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer has successfully changed requisites for help");
        
        return volunteer.Id.Value;
    }
}