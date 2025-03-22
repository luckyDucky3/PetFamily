using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Specie.SpecieRequests;
using PetFamily.API.Extensions;
using PetFamily.Application.Species.Commands;
using PetFamily.Application.Species.Commands.Create;
using PetFamily.Application.Species.Commands.Delete;

namespace PetFamily.API.Controllers.Specie;

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

    [HttpDelete("specie/{specieId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid specieId,
        [FromServices] DeleteHandler handler,
        CancellationToken cancellationToken = default)
    {
        var deleteCommand = new DeleteCommand(specieId);
        var result = await handler.Handle(deleteCommand, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
}