using FluentValidation;
using PetFamily.Application.Dtos.Validator;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.UploadFilesToPet;

public class UploadFilesValidator : AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleForEach(c => c.Files).SetValidator(new UploadFileDtoValidator());
    }
}