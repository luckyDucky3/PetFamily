using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.UpdateStatus;

public record UpdateStatusPetCommand(Guid VolunteerId, Guid PetId, Status PetStatus) : ICommand;

public class UpdateStatusPetHandler : ICommandHandler<Guid, UpdateStatusPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateStatusPetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStatusPetHandler(
        IVolunteersRepository volunteersRepository, 
        IValidator<UpdateStatusPetCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(UpdateStatusPetCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        VolunteerId vId = VolunteerId.Create(command.VolunteerId);
        
        var volunteer = await _volunteersRepository.GetById(vId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(vId.Value).ToErrorList()!;
        
        var pet = volunteer.Pets.FirstOrDefault(p => p.Id == command.PetId);
        if (pet == null)
            return Errors.General.IsNotFound(command.PetId).ToErrorList()!;
        
        pet.UpdateStatus(command.PetStatus);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        return pet.Id.Value;
    }
}