using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.SoftDelete;

namespace PetFamily.API.Controllers.Volunteer;


[ApiController]
[Route("[controller]")]
public class DeleteController : ControllerBase
{
    [HttpDelete("hard/{id:guid}/volunteer")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerHandler deleteVolunteerHandler,
        [FromServices] IValidator<HardDeleteVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new HardDeleteVolunteerRequest(id);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorsResponse();
        
        var command = new HardDeleteVolunteerCommand(id);

        var deleteVolunteerResult = await deleteVolunteerHandler.Handle(command, cancellationToken);
        if (deleteVolunteerResult.IsFailure)
            return deleteVolunteerResult.Error.ToResponse();
        
        return Ok(deleteVolunteerResult.Value);
    }
    
    [HttpDelete("soft/{id:guid}/volunteer")]
    public async Task<ActionResult<Guid>> SoftDelete(
        [FromRoute] Guid id,
        [FromServices] SoftDeleteVolunteerHandler softDeleteVolunteerHandler,
        [FromServices] IValidator<SoftDeleteVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new SoftDeleteVolunteerRequest(id);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorsResponse();
        
        var command = new SoftDeleteVolunteerCommand(id);

        var softDeleteVolunteerResult = await softDeleteVolunteerHandler.Handle(command, cancellationToken);
        if (softDeleteVolunteerResult.IsFailure)
            return softDeleteVolunteerResult.Error.ToResponse();
        
        return Ok(softDeleteVolunteerResult.Value);
    }
}