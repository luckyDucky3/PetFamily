using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.HardDelete;

public record HardDeleteVolunteerCommand(Guid Id);

public class HardDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;
    private readonly IValidator<HardDeleteVolunteerCommand> _validator;

    public HardDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeleteVolunteerHandler> logger,
        IValidator<HardDeleteVolunteerCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        HardDeleteVolunteerCommand volunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(volunteerCommand, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(volunteerCommand.Id);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result == null)
            return Errors.General.IsNotFound(volunteerCommand.Id).ToErrorList();

        var id = await _volunteersRepository.HardDelete(result, cancellationToken);

        _logger.LogInformation("Volunteer has been successfully deleted");

        return id;
    }
}