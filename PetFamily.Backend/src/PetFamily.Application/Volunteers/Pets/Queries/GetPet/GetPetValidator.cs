using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetPet;

public class GetPetValidator : AbstractValidator<GetPetQuery>
{
    public GetPetValidator()
    {
        RuleFor(q => q.FilePath).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(q => q.BucketName).NotEmpty().WithError(Errors.General.IsRequired());
    }
}