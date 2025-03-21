using System.Linq.Expressions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    FullNameDto? FullName,
    string? SortBy,
    bool? SortAscending,
    int Page,
    int PageSize) : IQuery;

public class GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteersWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Volunteers.AsQueryable();
        
        volunteersQuery = volunteersQuery.WhereIf(query.FullName != null, v => 
            v.Name.FirstName.StartsWith(query.FullName!.FirstName) &&
            v.Name.LastName.StartsWith(query.FullName!.LastName) &&
            (v.Name.Patronymic == null ||
            v.Name.Patronymic.StartsWith(query.FullName!.Patronymic!))
        );
        
        Expression<Func<VolunteerDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "first name" => v => v.Name.FirstName,
            "last name" => v => v.Name.LastName,
            "email" => v => v.EmailAddress,
            _ => v => v.Id
        };
        
        if (query.SortAscending ?? true)
            volunteersQuery = volunteersQuery.OrderBy(keySelector);
        else
            volunteersQuery = volunteersQuery.OrderByDescending(keySelector);
        
        return await volunteersQuery.ToPageList(query.Page, query.PageSize, cancellationToken);
    }
}