using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pet.PetRequests;
using PetFamily.API.Extensions;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.Pets.Commands.UpdatePet;
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
    
    // [HttpGet("pets/dapper")]
    // public async Task<ActionResult> GetPetsWithDapper(
    //     [FromQuery] GetPetsWithPaginationRequest request,
    //     [FromServices] GetPetsWithPaginationHandlerDapper handler,
    //     CancellationToken cancellationToken = default)
    // {
    //     var query = request.ToQuery();
    //     var pets = await handler.Handle(query, cancellationToken);
    //     return Ok(pets);
    // }
}