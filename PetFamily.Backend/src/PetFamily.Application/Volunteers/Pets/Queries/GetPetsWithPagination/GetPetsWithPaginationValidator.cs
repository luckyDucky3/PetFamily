using FluentValidation;
using PetFamily.Application.Database;
using PetFamily.Application.Validations;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationValidator : AbstractValidator<GetPetsWithPaginationQuery>
{
    public GetPetsWithPaginationValidator()
    {
        RuleFor(q => q.PageSize).NotEmpty().WithError(Errors.General.IsRequired());
        RuleFor(q => q.Page).NotEmpty().WithError(Errors.General.IsRequired());
    }
}