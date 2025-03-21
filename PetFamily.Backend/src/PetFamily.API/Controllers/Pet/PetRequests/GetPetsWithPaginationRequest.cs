using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record GetPetsWithPaginationRequest(
    string? Name,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    bool? SortAscending,
    int Page,
    int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() => new(Name, PositionFrom, PositionTo, SortBy, SortAscending, Page, PageSize);
}