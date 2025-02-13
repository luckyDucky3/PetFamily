using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Create;

namespace PetFamily.API.Controllers.Volunteer;

[ApiController]
[Route("[controller]")]
public class CreateController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(createVolunteerRequest, cancellationToken);

        var createVolunteerCommand = new CreateVolunteerCommand(
            createVolunteerRequest.FullName,
            createVolunteerRequest.Description,
            createVolunteerRequest.PhoneNumber,
            createVolunteerRequest.EmailAddress,
            createVolunteerRequest.ExperienceYears,
            createVolunteerRequest.SocialNetworks,
            createVolunteerRequest.RequisitesForHelp);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorsResponse();

        var result = await createVolunteerHandler.Handle(createVolunteerCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created(result.Value.ToString(), null);
    }
}