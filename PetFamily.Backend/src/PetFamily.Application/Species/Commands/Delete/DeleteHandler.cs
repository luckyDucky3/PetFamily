using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Commands.Delete;

public record DeleteCommand(Guid SpecieId) : ICommand;

public class DeleteHandler : ICommandHandler<Guid, DeleteCommand>
{
    private readonly ISpeciesRepository _repository;

    public DeleteHandler(ISpeciesRepository repository)
    {
       _repository = repository;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteCommand command, 
        CancellationToken cancellationToken = default)
    {
        var specieId = SpecieId.Create(command.SpecieId);
        var idResult = await _repository.Delete(specieId, cancellationToken);
        if (idResult.IsFailure)
            return Result.Failure<Guid, ErrorList>(idResult.Error);
        
        return idResult.Value;
    }
}