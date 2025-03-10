using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Pets.Commands.PetDtos;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files);

public class UploadFilesHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly ILogger<UploadFilesHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UploadFilesToPetCommand> _validator;

    public UploadFilesHandler(
        IFileProvider fileProvider,
        ILogger<UploadFilesHandler> logger,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UploadFilesToPetCommand> validator)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UploadFilesToPetCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        VolunteerId volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(command.VolunteerId).ToErrorList();

        var petResult = volunteer.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var pet = petResult.Value;

        List<FileData> fileDataUploads = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileData = new FileData(file.Stream, new FileInfo(filePath.Value, BUCKET_NAME));
            fileDataUploads.Add(fileData);
        }

        var filePathsResult = await _fileProvider.UploadFiles(fileDataUploads, cancellationToken);
        if (filePathsResult.IsFailure)
            return filePathsResult.Error.ToErrorList();

        var petFiles = filePathsResult.Value
            .Select(f => new PetFile(f))
            .ToList();

        pet.UpdateFiles(petFiles);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Uploaded files to pet - {id}", petResult.Value.Id.Value);

        return pet.Id.Value;
    }
}