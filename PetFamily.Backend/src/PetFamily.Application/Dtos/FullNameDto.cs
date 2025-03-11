using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

namespace PetFamily.Application.Dtos;

public class FullNameDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Patronymic { get; init; }
}
