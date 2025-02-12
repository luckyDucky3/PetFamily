using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.AddHelpRequisites;
using PetFamily.Application.Volunteers.AddSocialNetworks;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        [FromServices] IValidator<CreateVolunteerRequest> validator,
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
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