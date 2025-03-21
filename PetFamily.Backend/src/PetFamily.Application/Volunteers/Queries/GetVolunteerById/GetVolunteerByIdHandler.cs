using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid Id) : IQuery;

public class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto?, GetVolunteerByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteerByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<VolunteerDto?> Handle(
        GetVolunteerByIdQuery query, 
        CancellationToken cancellationToken = default)
    {
        return await _readDbContext.Volunteers.FirstOrDefaultAsync(
            v => v.Id == query.Id, cancellationToken);
    }
}