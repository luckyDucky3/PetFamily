using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Minio;
using PetFamily.API.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Pets.AddPet;
using PetFamily.Application.Pets.GetPet;
using PetFamily.Application.Pets.RemovePet;
using PetFamily.Infrastructure.Options;

namespace PetFamily.API.Controllers.Volunteer;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly IMinioClient _minioClient;

    public FileController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFile(
        IFormFile file,
        [FromServices] AddPetHandler addPetHandler,
        CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream();
        var fileData = new FileData(stream, "photos", Guid.NewGuid().ToString());
        
        var result = await addPetHandler.Handle(fileData, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpDelete]
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

    [HttpGet]
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