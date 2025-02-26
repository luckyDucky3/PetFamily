using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using PetFamily.Application.Volunteers._Validation;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithError(Errors.General.IsRequired("Id"));
    }
}

public class UpdateMainInfoDtoValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoDtoValidator()
    {
        RuleFor(u => u.Fullname).MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName, n.Patronymic));
        
        RuleFor(v => v.Description).MaximumLength(Constants.MAX_LONG_TEXT_LENGTH)
            .WithError(Errors.General.IsInvalid("Description"));
        
        RuleFor(u => u.EmailAddress).MustBeValueObject(EmailAddress.Create);
        
        RuleFor(u => u.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(v => v.ExperienceYears).Must(y => y >= 0 && y < Constants.MAX_EXP_YEARS)
            .WithError(Errors.General.IsInvalid("Experience years"));
    }
}