using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteers;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var createVolunteerCommand = new CreateVolunteerCommand(createVolunteerRequest);
        var result = await createVolunteerHandler.Handle(createVolunteerCommand, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Created(result.Value.ToString(), null);
    }
}