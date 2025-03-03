using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Pets.AddPet;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.GetPet;

public class GetPetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<GetPetHandler> _logger;

    public GetPetHandler(IFileProvider fileProvider, ILogger<GetPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(FileDataGet fileData, CancellationToken cancellationToken)
    {
        return await _fileProvider.GetFile(fileData, cancellationToken);
    }
}