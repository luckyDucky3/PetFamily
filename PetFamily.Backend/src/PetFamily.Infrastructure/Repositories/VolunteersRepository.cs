using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicatonDbContext _dbContext;
    public VolunteersRepository(ApplicatonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return (Guid)volunteer.Id;
    }

    public async Task<Result<Volunteer>> GetById(VolunteerId voluneerId, CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == voluneerId, cancellationToken);
        if (volunteer is null)
            return Result.Failure<Volunteer>("Volunteer not found");
        return Result.Success(volunteer);
    }

    public async Task<Result<Volunteer>> GetByFullName(FullName fullName, CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Name == fullName, cancellationToken);
        if (volunteer is null)
            return Result.Failure<Volunteer>("Volunteer not found");
        return Result.Success(volunteer);
    }
}