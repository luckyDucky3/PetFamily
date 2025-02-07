using FluentValidation;
using FluentValidation.AspNetCore;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteers;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(v => v.FullName)
            .MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName, n.Patronymic));

        RuleFor(v => v.EmailAddress).MustBeValueObject(EmailAddress.Create);

        RuleFor(v => v.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(v => v.Description).MaximumLength(Constants.MAX_LONG_TEXT_LENGTH)
            .WithError(Errors.General.IsInvalid("Description"));

        RuleFor(v => v.ExperienceYears).Must(y => y >= 0 && y < Constants.MAX_EXP_YEARS)
            .WithError(Errors.General.IsInvalid("Experience years"));

        RuleForEach(v => v.RequisitesForHelp).MustBeValueObject(r => RequisitesForHelp.Create(r.Title, r.Description));

        RuleForEach(v => v.SocialNetworks).MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Link));
    }
}