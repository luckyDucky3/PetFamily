using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.UpdateMainInfoPet;

public class UpdateMainInfoPetValidator : AbstractValidator<UpdateMainInfoPetCommand>
{
    public UpdateMainInfoPetValidator()
    {
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.PetMainInfoDto).NotEmpty();
    }
}