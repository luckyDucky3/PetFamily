using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SpeciesRepository(
        WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
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
    
}