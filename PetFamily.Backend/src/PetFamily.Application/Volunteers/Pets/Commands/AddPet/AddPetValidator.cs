using FluentValidation;
using PetFamily.Application.Validations;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.Volunteers.Pets.Commands.AddPet;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(a => a.AddressDto).MustBeValueObject(a => Address.Create(a.Country, a.City, a.Street, a.HomeNumber));
        RuleFor(a => a.Color).NotEmpty();
    }
}