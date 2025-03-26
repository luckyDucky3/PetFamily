using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Volunteer?> GetById(VolunteerId voluneerId, CancellationToken cancellationToken = default);
    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Guid> HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Volunteer?> GetByFullName(FullName fullName, CancellationToken cancellationToken = default);
    void FixDeletePet(Pet pet);
}