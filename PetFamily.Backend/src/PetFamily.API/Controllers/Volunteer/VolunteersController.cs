using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pet.PetRequests;
using PetFamily.API.Controllers.Processors;
using PetFamily.API.Controllers.Volunteer.VolunteerRequests;
using PetFamily.API.Extensions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Commands.AddHelpRequisites;
using PetFamily.Application.Volunteers.Commands.AddSocialNetworks;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.HardDelete;
using PetFamily.Application.Volunteers.Commands.SoftDelete;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Pets.Commands.AddPet;
using PetFamily.Application.Volunteers.Pets.Commands.RemoveFilePet;
using PetFamily.Application.Volunteers.Pets.Commands.RemovePet;
using PetFamily.Application.Volunteers.Pets.Commands.UpdateMainInfoPet;
using PetFamily.Application.Volunteers.Pets.Commands.UploadFilesToPet;
using PetFamily.Application.Volunteers.Pets.Queries.GetFilePet;
using PetFamily.Application.Volunteers.Queries.GetVolunteerById;
using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteer;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAllVolunteers(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var volunteers = await handler.Handle(query, cancellationToken);
        return Ok(volunteers);
    }

    [HttpGet("{id:guid}/volunteer")]
    public async Task<IActionResult> GetVolunteer(
        [FromRoute] Guid id,
        [FromServices] GetVolunteerByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerByIdQuery(id);
        var result = await handler.Handle(query, cancellationToken);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
    
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

        return Created(result.Value.ToString(), result.Value.ToString());
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
    

    [HttpDelete("file")]
    public async Task<IActionResult> DeleteFile(
        [FromBody] RemoveFileRequest request,
        [FromServices] RemoveFilePetHandler removeFilePetHandler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        var result = await removeFilePetHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("file-link")]
    public async Task<IActionResult> GetFileLink(
        [FromQuery] GetPetRequest request,
        [FromServices] GetFilePetHandler getFilePetHandler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        var result = await getFilePetHandler.Handle(query, cancellationToken);
        return Ok(result);
    }
}