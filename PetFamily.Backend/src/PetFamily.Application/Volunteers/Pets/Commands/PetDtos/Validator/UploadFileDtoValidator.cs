using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Commands.PetDtos.Validator;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(c => c.FileName).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(d => d.Stream).Must(s => s.Length < 10 * 1024 * 1024);
    }
}