using PetFamily.Application.Species.Commands.Create;

namespace PetFamily.API.Controllers.Specie.SpecieRequests;

public record CreateSpecieRequest(string SpecieName, IEnumerable<string> BreedNames)
{
    public CreateSpecieCommand ToCommand() 
        => new CreateSpecieCommand(SpecieName, BreedNames);
};