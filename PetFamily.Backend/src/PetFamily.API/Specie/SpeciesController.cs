using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Specie.SpecieRequests;
using PetFamily.Application.Species.Create;

namespace PetFamily.API.Specie;

[ApiController]
[Route("[controller]")]
public class SpeciesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateSpecieRequest request,
        [FromServices] CreateSpecieHandler handler,
        CancellationToken cancellationToken = default)
    {
        var createSpecieCommand = request.ToCommand();
        var result = await handler.Handle(createSpecieCommand, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Created(result.Value.ToString(), null);
    }
}