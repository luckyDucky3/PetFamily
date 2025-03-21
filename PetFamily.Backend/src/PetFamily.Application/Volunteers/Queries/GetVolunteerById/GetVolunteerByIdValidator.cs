using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithError(Errors.General.IsRequired());
    }
}