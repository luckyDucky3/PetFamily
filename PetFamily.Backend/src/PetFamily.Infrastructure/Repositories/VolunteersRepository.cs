using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _writeDbContext;

    public VolunteersRepository(
        WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Guid> Add(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _writeDbContext.SaveChangesAsync(cancellationToken);

        return (Guid)volunteer.Id;
    }

    public async Task<Volunteer?> GetById(
        VolunteerId voluneerId, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _writeDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == voluneerId, cancellationToken);
        
        return volunteer;
    }

    public async Task<Guid> Save(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _writeDbContext.Volunteers.Attach(volunteer);
        await _writeDbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }

    public async Task<Guid> HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _writeDbContext.Remove(volunteer);
        await _writeDbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }

    public async Task<Volunteer?> GetByFullName(
        FullName fullName, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _writeDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Name == fullName, cancellationToken);

        return volunteer;
    }
}