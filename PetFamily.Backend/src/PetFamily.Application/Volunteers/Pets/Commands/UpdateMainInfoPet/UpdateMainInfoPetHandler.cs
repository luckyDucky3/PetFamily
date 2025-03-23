using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.UpdateMainInfoPet;

public record UpdateMainInfoPetCommand(Guid VolunteerId, Guid PetId, PetMainInfoDto PetMainInfoDto) : ICommand;

public class UpdateMainInfoPetHandler : ICommandHandler<Guid, UpdateMainInfoPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<UpdateMainInfoPetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoPetCommand> _validator;

    public UpdateMainInfoPetHandler(
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        ILogger<UpdateMainInfoPetHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoPetCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoPetCommand command, 
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
        
        Pet? pet = volunteer.Pets.FirstOrDefault(p => p.Id.Value == command.PetId);
        if (pet == null)
            return Errors.General.IsNotFound(command.PetId).ToErrorList()!;
        
        var specieId = SpecieId.Create(command.PetMainInfoDto.SpecieId);
        var specie = await _speciesRepository.GetById(specieId, cancellationToken);
        if (specie == null)
            return Errors.General.IsNotFound(command.PetMainInfoDto.SpecieId).ToErrorList()!;
        
        var breed = specie.Breeds.FirstOrDefault(b => b.Id.Value == command.PetMainInfoDto.BreedId);
        if (breed == null)
            return Errors.General.IsNotFound(command.PetMainInfoDto.BreedId).ToErrorList()!;
        
        
        var speciesBreeds = SpeciesBreeds.Create(
            specieId, 
            BreedId.Create(command.PetMainInfoDto.BreedId)).Value;
        
        var dto = command.PetMainInfoDto;
        var address = Address.Create(
            dto.Address.Country,
            dto.Address.City,
            dto.Address.Street, 
            dto.Address.HomeNumber).Value;

        var phoneNumber = PhoneNumber.Create(dto.PhoneNumber ?? "").Value;
        
        pet.UpdateMainInfo(
            dto.Name,
            speciesBreeds,
            dto.Color,
            address,
            dto.Description,
            dto.InfoAboutHealth,
            dto.Weight,
            dto.Height,
            phoneNumber,
            dto.IsCastrate,
            dto.IsVaccinate,
            dto.BirthDate,
            dto.Status);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet is successfully updated");
        
        return pet.Id.Value;
    }
}