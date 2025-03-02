using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Processors;
using PetFamily.API.Controllers.Volunteer.VolunteerRequests;
using PetFamily.API.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers._Dto;
using PetFamily.Application.Volunteers.AddHelpRequisites;
using PetFamily.Application.Volunteers.AddSocialNetworks;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Application.Volunteers.Pets.AddPet;
using PetFamily.Application.Volunteers.Pets.GetPet;
using PetFamily.Application.Volunteers.Pets.RemovePet;
using PetFamily.Application.Volunteers.Pets.UploadFilesToPet;
using PetFamily.Application.Volunteers.SoftDelete;
using PetFamily.Application.Volunteers.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteer;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest createVolunteerRequest,
        [FromServices] CreateVolunteerHandler createVolunteerHandler,
        CancellationToken cancellationToken = default)
    {
        var createVolunteerCommand = createVolunteerRequest.ToCommand();

        var result = await createVolunteerHandler.Handle(createVolunteerCommand, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created(result.Value.ToString(), null);
    }

    [HttpDelete("hard/{id:guid}/volunteer")]
    public async Task<ActionResult<Guid>> Delete(
        [FromRoute] Guid id,
        [FromServices] HardDeleteVolunteerHandler deleteVolunteerHandler,
        CancellationToken cancellationToken = default)
    {
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
        CancellationToken cancellationToken = default)
    {
        var command = new SoftDeleteVolunteerCommand(id);

        var softDeleteVolunteerResult = await softDeleteVolunteerHandler.Handle(command, cancellationToken);
        if (softDeleteVolunteerResult.IsFailure)
            return softDeleteVolunteerResult.Error.ToResponse();

        return Ok(softDeleteVolunteerResult.Value);
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoRequest updateMainInfoRequest,
        [FromServices] UpdateMainInfoHandler updateMainInfoHandler,
        CancellationToken cancellationToken = default)
    {
        var command = updateMainInfoRequest.ToCommand(id);

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
        CancellationToken cancellationToken = default)
    {
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
        CancellationToken cancellationToken = default)
    {
        var command = new AddHelpRequisitesCommand(id, listHelpRequisiteDto.HelpRequisiteDtos);

        var addHelpRequisitesResult = await addHelpRequisitesHandler.Handle(command, cancellationToken);
        if (addHelpRequisitesResult.IsFailure)
            return addHelpRequisitesResult.Error.ToResponse();

        return Ok(addHelpRequisitesResult.Value);
    }


    [HttpPost("{id:guid}/pet")]
    public async Task<IActionResult> AddPet(
        [FromRoute] Guid id,
        [FromForm] AddPetRequest addPetRequest,
        [FromServices] AddPetHandler addPetHandler,
        CancellationToken cancellationToken = default)
    {
        var command = addPetRequest.ToCommand(id);
        
        var result = await addPetHandler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet/{petId:guid}/files")]
    public async Task<IActionResult> UploadFilesToPet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesHandler uploadFilesHandler,
        CancellationToken cancellationToken = default
        )
    {
        await using var formFileProcessor = new FormFileProcessor();
        var fileDtos = formFileProcessor.Process(files);
        
        var command = new UploadFilesToPetCommand(id, petId, fileDtos);
        
        var result = await uploadFilesHandler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }


    [HttpDelete("test-method")]
    public async Task<IActionResult> DeleteFile(
        [FromBody] FileDataRemove fileDataRemove,
        [FromServices] RemovePetHandler removePetHandler,
        CancellationToken cancellationToken = default)
    {
        var result = await removePetHandler.Handle(fileDataRemove, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("test-method")]
    public async Task<IActionResult> GetFile(
        [FromQuery] FileDataGet fileDataGet,
        [FromServices] GetPetHandler getPetHandler,
        CancellationToken cancellationToken = default)
    {
        var result = await getPetHandler.Handle(fileDataGet, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}