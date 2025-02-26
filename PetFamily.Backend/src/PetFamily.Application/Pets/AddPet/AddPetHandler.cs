using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Species;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using File = System.IO.File;

namespace PetFamily.Application.Pets.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string PetName,
    Guid SpecieId,
    Guid BreedId,
    Color Color,
    AddressDto AddressDto,
    IEnumerable<CreateFileDto> Files);

public record AddressDto(
    string Country,
    string City,
    string Street,
    string HomeNumber);

public record CreateFileDto(Stream Stream, string FileName);

public class AddPetHandler
{
    private const string BUCKET_NAME = "photos";
    
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;

    public AddPetHandler(IFileProvider fileProvider, ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository, IUnitOfWork unitOfWork, ISpeciesRepository speciesRepository)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(AddPetCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            VolunteerId volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
            if (volunteer == null)
                return Result.Failure<Guid, Error>(Errors.General.IsNotFound(command.VolunteerId));

            PetId petId = PetId.NewPetId();
            SpecieId specieId = SpecieId.Create(command.SpecieId);
            BreedId breedId = BreedId.Create(command.BreedId);
            var specie = await _speciesRepository.GetById(specieId, cancellationToken);
            if (specie == null)
                return Result.Failure<Guid, Error>(Errors.General.IsNotFound(command.SpecieId));

            var exist = specie.IsBreedExist(breedId);
            if (!exist)
                return Result.Failure<Guid, Error>(Errors.General.IsNotFound(command.BreedId));
            
            SpeciesBreeds sb = new SpeciesBreeds(specieId, breedId);
            var address = Address.Create(
                command.AddressDto.Country,
                command.AddressDto.City,
                command.AddressDto.Street,
                command.AddressDto.HomeNumber).Value;
            
            List<FileDataUpload> fileDataUploads = [];
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return Result.Failure<Guid, Error>(filePath.Error);

                var fileContent = new FileDataUpload(file.Stream, filePath.Value, BUCKET_NAME);
                fileDataUploads.Add(fileContent);
            }
            
            var petFiles = fileDataUploads.Select(f => f.FilePath)
                .Select(f => new PetFile(f))
                .ToList();

            var pet = Pet.Create(petId, command.PetName, sb, command.Color, address, files: petFiles).Value;
            
            var result = volunteer.AddPet(pet);
            if (result.IsFailure)
                return Result.Failure<Guid, Error>(result.Error);            
            
            var resultUploadFile = await _fileProvider.UploadFiles(fileDataUploads, cancellationToken);
            if (resultUploadFile.IsFailure)
                return Result.Failure<Guid, Error>(resultUploadFile.Error);
            
            await _unitOfWork.SaveChanges(cancellationToken);
            transaction.Commit();
            
            return Result.Success<Guid, Error>(pet.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Can not add pet to volunteer - {id} in transaction", command.VolunteerId);
            
            transaction.Rollback();
            
            return Error.Failure($"Can not add pet to volunteer - {command.VolunteerId}", "volunteer.pet.failure");
        }
    }
}