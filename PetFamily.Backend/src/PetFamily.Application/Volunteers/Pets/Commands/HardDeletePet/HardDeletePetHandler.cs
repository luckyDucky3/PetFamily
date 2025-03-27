using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Commands.HardDeletePet;

public record HardDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;

public class HardDeletePetHandler : ICommandHandler<Guid, HardDeletePetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<HardDeletePetCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<HardDeletePetHandler> _logger;

    public HardDeletePetHandler(
        IVolunteersRepository repository,
        IValidator<HardDeletePetCommand> validator,
        IUnitOfWork unitOfWork,
        IFileProvider fileProvider,
        ILogger<HardDeletePetHandler> logger)
    {
        _volunteersRepository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(HardDeletePetCommand petCommand,
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
        
        volunteer.RemovePet(pet);
        _volunteersRepository.FixDeletePet(pet);

        foreach (var f in pet.Files)
        {
            await _fileProvider.DeleteFile(new FileInfo(f.PathToStorage, Constants.BUCKET_NAME),cancellationToken);
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet was successfully hard deleted");
        return petId.Value;
    }
}