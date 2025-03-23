using FluentValidation;
using PetFamily.Application.Database;

namespace PetFamily.Application.Volunteers.Pets.Commands.UpdatePet;

public class UpdateMainInfoPetValidator : AbstractValidator<UpdateMainInfoPetCommand>
{
    public UpdateMainInfoPetValidator()
    {
        
    }
}