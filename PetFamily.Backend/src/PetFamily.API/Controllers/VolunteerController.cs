using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
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
        [FromServices] IValidator<CreateVolunteerCommand> validator,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        CancellationToken cancellationToken = default)
    {
        var createVolunteerCommand = new CreateVolunteerCommand(createVolunteerRequest);

        var validationResult = await validator.ValidateAsync(createVolunteerCommand, cancellationToken);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors;

            var errors = from validationError in validationErrors
                let error = Error.Validation(validationError.ErrorCode, validationError.ErrorMessage)
                select new ResponseError(error.Code, error.Message, validationError.PropertyName);

            var envelope = Envelope.Error(errors);
            return BadRequest(envelope);
        }

        var result = await createVolunteerHandler.Handle(createVolunteerCommand, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created(result.Value.ToString(), null);
    }
}