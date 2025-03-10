using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(int Page, int PageSize);

public class GetPetsWithPaginationHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetPetsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _readDbContext.Pets.AsQueryable();
        return await petsQuery.ToPageList(query.Page, query.PageSize, cancellationToken);
    }
}