using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Commands.ChooseGeneralPhoto;

public record ChooseGeneralPhotoCommand(Guid VolunteerId, Guid PetId, string FilePath) : ICommand;

public class ChooseGeneralPhotoHandler : ICommandHandler<string, ChooseGeneralPhotoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<ChooseGeneralPhotoCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChooseGeneralPhotoHandler> _logger;

    public ChooseGeneralPhotoHandler(
        IVolunteersRepository repository,
        IValidator<ChooseGeneralPhotoCommand> validator,
        IUnitOfWork unitOfWork,
        ILogger<ChooseGeneralPhotoHandler> logger)
    {
        _volunteersRepository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> Handle(
        ChooseGeneralPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var petId = PetId.Create(command.PetId);

        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(command.VolunteerId).ToErrorList()!;

        var pet = volunteer.Pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.IsNotFound(command.PetId).ToErrorList()!;

        var photo = pet.Files.FirstOrDefault(p => command.FilePath == p.PathToStorage.Path.Split('.')[0]);
        if (photo == null)
            return Errors.General.IsNotFound().ToErrorList()!;

        var generalPhoto = pet.Files.FirstOrDefault(p => command.FilePath == p.PathToStorage.Path);
        if (generalPhoto != null)
        {
            generalPhoto = new PetFile(generalPhoto.PathToStorage, false);
            pet.ChangeExistFile(generalPhoto);
        }

        photo = new PetFile(photo.PathToStorage, true);

        pet.ChangeExistFile(photo);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Photo was successfully choose general.");

        return command.FilePath;
    }
}