using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.HardDelete;

public class HardDeleteVolunteerValidator : AbstractValidator<HardDeleteVolunteerCommand>
{
    public HardDeleteVolunteerValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithError(Errors.General.IsRequired("Id"));
    }
}