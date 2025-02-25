using FluentValidation;
using PetFamily.Application.Volunteers._Validation;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.Pets.AddPet;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(a => a.AddressDto).MustBeValueObject(a => Address.Create(a.Country, a.City, a.Street, a.HomeNumber));
        RuleFor(a => a.Color).NotEmpty();
    }
}