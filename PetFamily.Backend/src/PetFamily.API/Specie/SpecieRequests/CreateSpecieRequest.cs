using PetFamily.Application.Species.Create;

namespace PetFamily.API.Specie.SpecieRequests;

public record CreateSpecieRequest(string SpecieName, IEnumerable<string> BreedNames)
{
    public CreateSpecieCommand ToCommand() 
        => new CreateSpecieCommand(SpecieName, BreedNames);
};