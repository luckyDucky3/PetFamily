using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public record DeleteVolunteerRequest(Guid Id);

public record DeleteVolunteerCommand(Guid Id);

public class DeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        DeleteVolunteerCommand volunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(volunteerCommand.Id);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound(volunteerCommand.Id));

        var id = await _volunteersRepository.Delete(result, cancellationToken);

        _logger.LogInformation("Volunteer has been successfully deleted");

        return Result.Success<Guid, Error>(id);
    }
}