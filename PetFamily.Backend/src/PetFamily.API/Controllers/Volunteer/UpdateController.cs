using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers._Dto;
using PetFamily.Application.Volunteers.AddHelpRequisites;
using PetFamily.Application.Volunteers.AddSocialNetworks;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteer;


[ApiController]
[Route("[controller]")]

public class UpdateController : ControllerBase
{
    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoDto updateMainInfoDto,
        [FromServices] UpdateMainInfoHandler updateMainInfoHandler,
        [FromServices] IValidator<UpdateMainInfoRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateMainInfoRequest(id, updateMainInfoDto);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorsResponse();
        
        var command = new UpdateMainInfoCommand(
            id,
            updateMainInfoDto.Fullname,
            updateMainInfoDto.Description,
            updateMainInfoDto.EmailAddress,
            updateMainInfoDto.PhoneNumber,
            updateMainInfoDto.ExperienceYears);

        var updateResult = await updateMainInfoHandler.Handle(command, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult.Error.ToResponse();

        return Ok(updateResult.Value);
    }

    [HttpPatch("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> AddSocialNetworks(
        [FromRoute] Guid id,
        [FromBody] ListSocialNetworkDto socialNetworksDto,
        [FromServices] AddSocialNetworksHandler addSocialNetworksHandler,
        [FromServices] IValidator<AddSocialNetworksRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new AddSocialNetworksRequest(id, socialNetworksDto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorsResponse();

        var command = new AddSocialNetworksCommand(id, socialNetworksDto.SocialNetworkDtos);
        
        var addSocialNetworksResult = await addSocialNetworksHandler.Handle(command, cancellationToken);
        if (addSocialNetworksResult.IsFailure)
            return addSocialNetworksResult.Error.ToResponse();

        return Ok(addSocialNetworksResult.Value);
    }

    [HttpPatch("{id:guid}/help-requisites")]
    public async Task<ActionResult<Guid>> AddHelpRequisites(
        [FromRoute] Guid id,
        [FromBody] ListHelpRequisiteDto listHelpRequisiteDto,
        [FromServices] AddHelpRequisitesHandler addHelpRequisitesHandler,
        [FromServices] IValidator<AddHelpRequisitesRequest> validator,
        CancellationToken cancellationToken = default)
    {
        var request = new AddHelpRequisitesRequest(id, listHelpRequisiteDto);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorsResponse();

        var command = new AddHelpRequisitesCommand(id, listHelpRequisiteDto.HelpRequisiteDtos);
        
        var addHelpRequisitesResult = await addHelpRequisitesHandler.Handle(command, cancellationToken);
        if (addHelpRequisitesResult.IsFailure)
            return addHelpRequisitesResult.Error.ToResponse();

        return Ok(addHelpRequisitesResult.Value);
    }
}