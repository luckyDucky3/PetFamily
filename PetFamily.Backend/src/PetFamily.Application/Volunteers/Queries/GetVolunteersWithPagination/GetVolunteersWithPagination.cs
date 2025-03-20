using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    FullNameDto? fullName,
    int Page,
    int PageSize) : IQuery;

public class GetVolunteersWithPagination : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteersWithPagination(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Volunteers.AsQueryable();
        
        volunteersQuery = volunteersQuery.WhereIf(query.fullName != null, v => v.Name == query.fullName);
        
        return await volunteersQuery.ToPageList(query.Page, query.PageSize, cancellationToken);
    }
}