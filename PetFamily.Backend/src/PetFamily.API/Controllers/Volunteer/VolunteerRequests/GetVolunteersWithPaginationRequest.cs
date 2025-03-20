using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record GetVolunteersWithPaginationRequest(FullNameDto? FullName,
    int Page,
    int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery() => new(FullName, Page, PageSize);
}