using FluentValidation;
using PetFamily.Application.Volunteers._Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.SoftDelete;

public class SoftDeleteVolunteerValidator : AbstractValidator<SoftDeleteVolunteerRequest>
{
    public SoftDeleteVolunteerValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.IsRequired("Id"));
    }
}
