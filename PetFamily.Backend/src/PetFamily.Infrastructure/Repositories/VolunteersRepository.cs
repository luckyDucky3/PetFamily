using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicatonDbContext _dbContext;

    public VolunteersRepository(
        ApplicatonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return (Guid)volunteer.Id;
    }

    public async Task<Volunteer?> GetById(
        VolunteerId voluneerId, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == voluneerId, cancellationToken);
        
        return volunteer;
    }

    public async Task<Guid> Save(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }

    public async Task<Guid> HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }

    public async Task<Volunteer?> GetByFullName(
        FullName fullName, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Name == fullName, cancellationToken);

        return volunteer;
    }
}