using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _writeDbContext;
    private readonly ReadDbContext _readDbContext;

    public SpeciesRepository(
        WriteDbContext writeDbContext,
        ReadDbContext readDbContext
    )
    {
        _writeDbContext = writeDbContext;
        _readDbContext = readDbContext;
    }

    public async Task<Guid> Add(
        Specie specie,
        CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Species.AddAsync(specie, cancellationToken);
        await _writeDbContext.SaveChangesAsync(cancellationToken);

        return specie.Id.Value;
    }

    public async Task<Specie?> GetById(
        SpecieId specieId,
        CancellationToken cancellationToken = default)
    {
        var specie = await _writeDbContext.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken);

        return specie;
    }

    public async Task<Result<Guid, Error>> Delete(
        SpecieId specieId,
        CancellationToken cancellationToken = default)
    {
        var specie = await GetById(specieId, cancellationToken);
        if (specie == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound(specieId.Value));

        PetDto? resultPet = await _readDbContext.Pets.FirstOrDefaultAsync(
                p => p.SpecieId == specieId.Value, cancellationToken);
        if (resultPet != null)
            return Error.Failure("pet_has_this_specie", "Pets should not have this specie");

        _writeDbContext.Species.Remove(specie);
        await _writeDbContext.SaveChangesAsync(cancellationToken);
        return specie.Id.Value;
    }
}