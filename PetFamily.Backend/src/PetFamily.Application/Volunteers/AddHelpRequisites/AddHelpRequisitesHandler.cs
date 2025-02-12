using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.AddSocialNetworks;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddHelpRequisites;

public record AddHelpRequisitesRequest(Guid Id, ListHelpRequisiteDto HelpRequisiteDtos);

public record AddHelpRequisitesCommand(Guid Id, List<HelpRequisiteDto> HelpRequisiteDtos);

public class AddHelpRequisitesHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddSocialNetworksHandler> _logger;

    public AddHelpRequisitesHandler(
        IVolunteersRepository volunteersRepository, 
        ILogger<AddSocialNetworksHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddHelpRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound(volunteerId.Value));

        List<HelpRequisite> helpRequisites = [];
        helpRequisites.AddRange(command.HelpRequisiteDtos.Select(helpRequisiteDto =>
            HelpRequisite.Create(helpRequisiteDto.Title, helpRequisiteDto.Description).Value));

        volunteer.UpdateHelpRequisites(helpRequisites);
        
        await _volunteersRepository.Save(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer has successfully changed requisites for help");
        
        return volunteer.Id.Value;
    }
}