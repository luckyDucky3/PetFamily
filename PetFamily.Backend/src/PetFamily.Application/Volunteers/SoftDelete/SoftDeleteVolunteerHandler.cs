using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.SoftDelete;

public record SoftDeleteVolunteerCommand(Guid Id);

public class SoftDeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<SoftDeleteVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<SoftDeleteVolunteerHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        SoftDeleteVolunteerCommand volunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(volunteerCommand.Id);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound(volunteerCommand.Id));

        result.Deactivate();
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer has been successfully deactivate");
        
        return Result.Success<Guid, Error>(volunteerId);
    }
}