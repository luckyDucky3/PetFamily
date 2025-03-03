using FluentValidation;
using FluentValidation.AspNetCore;
using PetFamily.Application.Volunteers._Validation;
using PetFamily.Application.Volunteers.Pets.PetDtos.Validator;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.UploadFilesToPet;

public class UploadFilesValidator : AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.IsRequired());
        RuleForEach(c => c.Files).SetValidator(new UploadFileDtoValidator());
    }
}