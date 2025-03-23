using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.HardDeletePet;


public record HardDeleteCommand(Guid VolunteerId, Guid PetId) : ICommand;

public class HardDeleteHandler : ICommandHandler<Guid, HardDeleteCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(HardDeleteCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}