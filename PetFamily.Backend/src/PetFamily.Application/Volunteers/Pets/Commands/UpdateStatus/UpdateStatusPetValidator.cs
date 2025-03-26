using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.UpdateStatus;

public class UpdateStatusPetValidator : AbstractValidator<UpdateStatusPetCommand>
{
    public UpdateStatusPetValidator()
    {
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.PetStatus).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.PetStatus).Must(s => s == Status.SearchHome || s == Status.Sick)
            .WithError(Errors.General.IsInvalid("status"));
    }
}