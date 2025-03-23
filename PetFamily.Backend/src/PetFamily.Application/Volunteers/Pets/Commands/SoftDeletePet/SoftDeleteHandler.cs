using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.SoftDeletePet;

public record SoftDeleteCommand(Guid VolunteerId, Guid PetId) : ICommand;

public class SoftDeleteHandler : ICommandHandler<Guid, SoftDeleteCommand>
{
    public Task<Result<Guid, ErrorList>> Handle(SoftDeleteCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}