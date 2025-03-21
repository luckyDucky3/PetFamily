using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Create;

public record CreateSpecieCommand(string SpecieName, IEnumerable<string> BreedNames) : ICommand;

public class CreateSpecieHandler : ICommandHandler<Guid, CreateSpecieCommand>
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;
    private readonly IValidator<CreateSpecieCommand> _validator;

    public CreateSpecieHandler(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpecieHandler> logger,
        IValidator<CreateSpecieCommand> validator)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();

        var specieId = SpecieId.Create(Guid.NewGuid());
        var specie = Specie.Create(specieId, command.SpecieName).Value;
        
        List<Breed> breeds = [];
        breeds.AddRange(
            from breedName in command.BreedNames
            let breedId = BreedId.Create(Guid.NewGuid())
            select Breed.Create(breedId, breedName).Value);
        
        specie.AddBreeds(breeds);
        var id = await _speciesRepository.Add(specie, cancellationToken);
        
        _logger.LogInformation("Specie has been successfully added");
        
        return id;
    }
}