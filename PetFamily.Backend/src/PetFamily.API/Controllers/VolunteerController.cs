using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.CreateVolunteers;
using PetFamily.Domain.Shared;

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
        var result = await createVolunteerHandler.Handle(createVolunteerRequest, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
}