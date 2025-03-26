using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Application.Volunteers.Pets.Commands.RemoveFilePet;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.RemovePet;

public class RemovePetValidator : AbstractValidator<RemoveFilePetQuery>
{
    public RemovePetValidator()
    {
        RuleFor(q => q.FilePath).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(q => q.BucketName).NotEmpty().WithError(Errors.General.IsRequired());
    }
}