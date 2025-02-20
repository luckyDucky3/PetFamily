using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Pets.AddPet;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.GetPet;

public class GetPetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;

    public GetPetHandler(IFileProvider fileProvider, ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(FileDataGet fileData, CancellationToken cancellationToken)
    {
        return await _fileProvider.GetFile(fileData, cancellationToken);
    }
}