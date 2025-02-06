using FluentValidation;
using FluentValidation.AspNetCore;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteers;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(v => new { v.FirstName, v.LastName, v.Patronymic })
            .MustBeValueObject(n => FullName.Create(n.FirstName, n.LastName, n.Patronymic));

        RuleFor(v => v.EmailAddress).MustBeValueObject(EmailAddress.Create);

        RuleFor(v => v.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleFor(v => v.Description).NotNull().MaximumLength(Constants.MAX_LONG_TEXT_LENGTH);

        RuleFor(v => v.ExperienceYears).NotNull().Must(v => v < Constants.MAX_EXP_YEARS);

        RuleForEach(v => v.RequisitesForHelp).MustBeValueObject(r => RequisitesForHelp.Create(r.Title, r.Description));

        RuleForEach(v => v.SocialNetworks).MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Link));
    }
}