using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicatonDbContext _dbContext;

    public SpeciesRepository(
        ApplicatonDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Guid> Add(
        Specie specie, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Species.AddAsync(specie, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return specie.Id.Value;
    }
    
    public async Task<Specie?> GetById(
        SpecieId specieId, 
        CancellationToken cancellationToken = default)
    {
        var specie = await _dbContext.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken);
        
        return specie;
    }
    
}