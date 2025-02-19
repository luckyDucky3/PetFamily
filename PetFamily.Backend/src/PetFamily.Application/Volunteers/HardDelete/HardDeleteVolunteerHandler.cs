using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.HardDelete;

public record HardDeleteVolunteerRequest(Guid Id);

public record HardDeleteVolunteerCommand(Guid Id);

public class HardDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<HardDeleteVolunteerHandler> _logger;

    public HardDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<HardDeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        HardDeleteVolunteerCommand volunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(volunteerCommand.Id);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound(volunteerCommand.Id));

        var id = await _volunteersRepository.HardDelete(result, cancellationToken);

        _logger.LogInformation("Volunteer has been successfully deleted");

        return Result.Success<Guid, Error>(id);
    }
}