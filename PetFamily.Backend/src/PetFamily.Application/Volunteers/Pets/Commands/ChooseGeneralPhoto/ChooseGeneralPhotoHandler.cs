using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.ChooseGeneralPhoto;

public record ChooseGeneralPhotoCommand(Guid VolunteerId, Guid PetId) : ICommand;

public class ChooseGeneralPhotoHandler : ICommandHandler<Guid, ChooseGeneralPhotoCommand>
{
    public Task<Result<Guid, ErrorList>> Handle(ChooseGeneralPhotoCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}