using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pet.PetRequests;

public record GetPetsWithPaginationRequest(int Page, int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() => new(Page, PageSize);
}