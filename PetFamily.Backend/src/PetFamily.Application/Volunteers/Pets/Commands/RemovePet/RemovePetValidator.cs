using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.RemovePet;

public class RemovePetValidator : AbstractValidator<RemovePetQuery>
{
    public RemovePetValidator()
    {
        RuleFor(q => q.FilePath).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(q => q.BucketName).NotEmpty().WithError(Errors.General.IsRequired());
    }
}