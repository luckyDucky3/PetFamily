using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Providers;
using PetFamily.Application.Species;
using PetFamily.Application.Volunteers.Pets.PetDtos;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string PetName,
    Guid SpecieId,
    Guid BreedId,
    Color Color,
    AddressDto AddressDto);

public class AddPetHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository, 
        IUnitOfWork unitOfWork, 
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPetCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        VolunteerId volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(command.VolunteerId).ToErrorList();

        var petResult = await InitPet(command, cancellationToken);
        if (!petResult.IsSuccess)
            return petResult.Error.ToErrorList();
        
        var pet = petResult.Value;
        
        var result = volunteer.AddPet(pet);
        if (result.IsFailure)
            return new ErrorList([result.Error]);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return pet.Id.Value;
    }

    private async Task<Result<Pet, Error>> InitPet(AddPetCommand command, CancellationToken cancellationToken = default)
    {
        PetId petId = PetId.NewPetId();
        SpecieId specieId = SpecieId.Create(command.SpecieId);
        BreedId breedId = BreedId.Create(command.BreedId);
        var specie = await _speciesRepository.GetById(specieId, cancellationToken);
        if (specie == null)
            return Result.Failure<Pet, Error>(Errors.General.IsNotFound(command.SpecieId));

        var exist = specie.IsBreedExist(breedId);
        if (!exist)
            return Result.Failure<Pet, Error>(Errors.General.IsNotFound(command.BreedId));
        
        SpeciesBreeds sb = new SpeciesBreeds(specieId, breedId);
        var address = Address.Create(
            command.AddressDto.Country,
            command.AddressDto.City,
            command.AddressDto.Street,
            command.AddressDto.HomeNumber).Value;

        var pet = Pet.Create(petId, command.PetName, sb, command.Color, address).Value;

        return pet;
    }
}