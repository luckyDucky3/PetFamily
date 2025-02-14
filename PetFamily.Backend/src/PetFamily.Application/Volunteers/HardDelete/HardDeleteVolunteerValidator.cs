using FluentValidation;
using PetFamily.Application.Volunteers._Validation;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete;

public class HardDeleteVolunteerValidator : AbstractValidator<HardDeleteVolunteerRequest>
{
    public HardDeleteVolunteerValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.IsRequired("Id"));
    }
}