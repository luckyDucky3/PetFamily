using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;

public class SoftDeletePetHandler : ICommandHandler<Guid, SoftDeletePetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<SoftDeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SoftDeletePetHandler> _logger;

    public SoftDeletePetHandler(
        IVolunteersRepository repository,
        IValidator<SoftDeletePetCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeletePetHandler> logger)
    {
        _volunteersRepository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeletePetCommand petCommand, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(petCommand, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerId = VolunteerId.Create(petCommand.VolunteerId);
        var petId = PetId.Create(petCommand.PetId);

        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(petCommand.VolunteerId).ToErrorList()!;

        var pet = volunteer.Pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.IsNotFound(petCommand.PetId).ToErrorList()!;
        
        pet.Deactivate();
        await _unitOfWork.SaveChanges(cancellationToken);
        _logger.LogInformation("Volunteer has been successfully soft deleted");

        return pet.Id.Value;
    }
}