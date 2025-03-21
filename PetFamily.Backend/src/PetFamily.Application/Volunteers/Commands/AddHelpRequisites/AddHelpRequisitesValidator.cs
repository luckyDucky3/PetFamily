using FluentValidation;
using PetFamily.Application.Dtos;
using PetFamily.Application.Validations;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.AddHelpRequisites;

public class AddHelpRequisitesValidator : AbstractValidator<AddHelpRequisitesCommand>
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