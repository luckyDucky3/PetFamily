using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pet.PetRequests;
using PetFamily.API.Response;
using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pet;

[ApiController]
[Route("[controller]")]
public class PetsController : ApplicationController
{
    [HttpGet("pets")]
    public async Task<ActionResult> GetPets(
        [FromQuery] GetPetsWithPaginationRequest request,
        [FromServices] GetPetsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        var pets = await handler.Handle(query, cancellationToken);
        return Ok(pets);
    }
}