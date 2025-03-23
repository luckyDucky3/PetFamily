using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    public Task<Guid> Add(
        Specie specie,
        CancellationToken cancellationToken = default);

    public Task<Specie?> GetById(
        SpecieId specieId,
        CancellationToken cancellationToken = default);
    public Task<Result<Guid, Error>> Delete(
        SpecieId specieId, 
        CancellationToken cancellationToken = default);
}