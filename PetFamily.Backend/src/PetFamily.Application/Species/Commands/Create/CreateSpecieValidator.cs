using FluentValidation;

namespace PetFamily.Application.Species.Commands.Create;

public class CreateSpecieValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpecieValidator()
    {
        RuleFor(c => c.SpecieName).NotEmpty().WithMessage("Specie name cannot be empty");
        RuleForEach(c => c.BreedNames).NotEmpty().WithMessage("Breed names cannot be empty");
    }
}