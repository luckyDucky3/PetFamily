using System.Linq.Expressions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
    string? Name,
    int? FromPosition,
    int? ToPosition,
    string? SortBy,
    bool? SortAscending,
    int Page,
    int PageSize)
    : IQuery;

public class GetPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
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

        petsQuery = petsQuery.WhereIf(query.Name != null, p => p.Name.StartsWith(query.Name!));
        petsQuery = petsQuery.WhereIf(query.FromPosition != null, p => p.Position >= query.FromPosition);
        petsQuery = petsQuery.WhereIf(query.ToPosition != null, p => p.Position <= query.ToPosition);

        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => p => p.Name,
            "height" => p => p.Height ?? 0,
            _ => p => p.Id
        };
        
        if (query.SortAscending ?? true)
            petsQuery = petsQuery.OrderBy(keySelector);
        else
            petsQuery = petsQuery.OrderByDescending(keySelector);
        
        return await petsQuery.ToPageList(query.Page, query.PageSize, cancellationToken);
    }
}

//тестовый Handler, не участвующий в проекте
// public class GetPetsWithPaginationHandlerDapper : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
// {
//     private readonly ISqlConnectionFactory _sqlConnectionFactory;
//
//     public GetPetsWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
//     {
//         _sqlConnectionFactory = sqlConnectionFactory;
//     }
//
//     public async Task<PagedList<PetDto>> Handle(GetPetsWithPaginationQuery query,
//         CancellationToken cancellationToken = default)
//     {
//         var connection = _sqlConnectionFactory.Create();
//
//         var parameters = new DynamicParameters();
//         parameters.Add("@PageSize", query.PageSize);
//         parameters.Add("@Offset", query.PageSize * (query.Page - 1));
//
//         var totalCount = await connection.ExecuteScalarAsync<long>("SELECT Count(*) FROM pet");
//
//         var sql = """
//                   SELECT pet.id, pet.name, pet.birth_date, pet.files
//                   FROM pet
//                   ORDER BY pet.position
//                   LIMIT @PageSize
//                   OFFSET @Offset
//                   """;
//         var pets = await connection.QueryAsync<PetDto, string, PetDto>(
//             sql,
//             (pet, jsonFiles) =>
//             {
//                 var files = JsonSerializer.Deserialize<PetFileDto[]>(jsonFiles) ?? [];
//                 pet.Files = files;
//                 return pet;
//             },
//             splitOn: "files",
//             param: parameters);
//
//         return new PagedList<PetDto>()
//         {
//             TotalCount = totalCount,
//             Items = pets.ToList(),
//             PageSize = query.PageSize,
//             Page = query.Page
//         };
//     }
//}