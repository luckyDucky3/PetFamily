using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pet.PetRequests;
using PetFamily.API.Controllers.Processors;
using PetFamily.API.Controllers.Volunteer.VolunteerRequests;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.Pets.Commands.AddPet;
using PetFamily.Application.Volunteers.Pets.Commands.ChooseGeneralPhoto;
using PetFamily.Application.Volunteers.Pets.Commands.HardDeletePet;
using PetFamily.Application.Volunteers.Pets.Commands.SoftDeletePet;
using PetFamily.Application.Volunteers.Pets.Commands.UpdateMainInfoPet;
using PetFamily.Application.Volunteers.Pets.Commands.UploadFilesToPet;
using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Controllers.Pet;

[ApiController]
[Route("[controller]")]
public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAllPets(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        var pets = await handler.Handle(query, cancellationToken);
        return Ok(pets);
    }
    
    [HttpPost("{volunteerId:guid}/pet")]
    public async Task<IActionResult> AddPet(
        [FromRoute] Guid volunteerId,
        [FromBody] AddPetRequest addPetRequest,
        [FromServices] AddPetHandler addPetHandler,
        CancellationToken cancellationToken = default)
    {
        var command = addPetRequest.ToCommand(volunteerId);
        
        var result = await addPetHandler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
    public async Task<IActionResult> UploadFilesToPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesHandler uploadFilesHandler,
        CancellationToken cancellationToken = default
    )
    {
        await using var formFileProcessor = new FormFileProcessor();
        var fileDtos = formFileProcessor.Process(files);
        
        var command = new UploadFilesToPetCommand(volunteerId, petId, fileDtos);
        
        var result = await uploadFilesHandler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPut("{volunteerId}/pet/{petId:guid}")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdateMainInfoPetRequest request,
        [FromServices] UpdateMainInfoPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId, request.MainInfoDto);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpDelete("/soft/{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult> SoftDelete(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] SoftDeletePetHandler petHandler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new SoftDeletePetCommand(volunteerId, petId);
        var result = await petHandler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpDelete("/hard/{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult> HardDelete(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardDeletePetHandler handler,
        CancellationToken cancellationToken = default
    )
    {
        var command = new HardDeletePetCommand(volunteerId, petId);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPatch("/main-photo/{volunteerId:guid}/pet/{petId:guid}")]
    public async Task<ActionResult> ChooseGeneralPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] ChooseGeneralPhotoRequest request,
        [FromServices] ChooseGeneralPhotoHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = request.ToCommand(volunteerId, petId);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}