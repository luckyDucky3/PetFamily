using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetById(VolunteerId voluneerId, CancellationToken cancellationToken = default);
}