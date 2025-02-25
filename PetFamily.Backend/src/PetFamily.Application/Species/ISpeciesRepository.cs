using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Application.Species;

public interface ISpeciesRepository
{
    public Task<Guid> Add(
        Specie specie,
        CancellationToken cancellationToken = default);

    public Task<Specie?> GetById(
        SpecieId specieId,
        CancellationToken cancellationToken = default);

    public Task<bool> IsExist(
        SpecieId specieId,
        BreedId breedId,
        CancellationToken cancellationToken = default);
}