using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record GetPetsWithPaginationRequest(string? Name, int? PositionFrom, int? PositionTo, int Page, int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() => new(Name, PositionFrom, PositionTo, Page, PageSize);
}