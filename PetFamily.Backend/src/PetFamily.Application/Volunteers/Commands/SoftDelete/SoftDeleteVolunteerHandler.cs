using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.SoftDelete;

public record SoftDeleteVolunteerCommand(Guid Id) : ICommand;

public class SoftDeleteVolunteerHandler : ICommandHandler<Guid, SoftDeleteVolunteerCommand>
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

    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeleteVolunteerCommand volunteerCommand,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(volunteerCommand.Id);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result == null)
            return Errors.General.IsNotFound(volunteerCommand.Id).ToErrorList()!;

        result.Deactivate();
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer has been successfully deactivate");
        
        return Result.Success<Guid, ErrorList>(volunteerId);
    }
}