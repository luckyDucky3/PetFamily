using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetFilePet;

public class GetFilePetValidator : AbstractValidator<GetPetQuery>
{
    public GetFilePetValidator()
    {
        RuleFor(q => q.FilePath).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(q => q.BucketName).NotEmpty().WithError(Errors.General.IsRequired());
    }
}