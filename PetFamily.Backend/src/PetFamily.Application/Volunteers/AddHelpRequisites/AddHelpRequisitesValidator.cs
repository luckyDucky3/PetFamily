using FluentValidation;
using PetFamily.Application.Volunteers._Dto;
using PetFamily.Application.Volunteers._Validation;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddHelpRequisites;

public class AddHelpRequisitesValidator : AbstractValidator<AddHelpRequisitesRequest>
{
    public AddHelpRequisitesValidator()
    {
        RuleFor(a => a.Id).NotEmpty().WithError(Errors.General.IsRequired("Id"));
    }
}

public class AddHelpRequisitesDtoValidator : AbstractValidator<ListHelpRequisiteDto>
{
    public AddHelpRequisitesDtoValidator()
    {
        RuleForEach(l => l.HelpRequisiteDtos).MustBeValueObject(r => SocialNetwork.Create(r.Title, r.Description));
    }
}