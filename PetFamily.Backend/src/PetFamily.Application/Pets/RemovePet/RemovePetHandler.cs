using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Pets.AddPet;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.RemovePet;

public class RemovePetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;

    public RemovePetHandler(IFileProvider fileProvider, ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(FileDataRemove fileData, CancellationToken cancellationToken)
    {
        return await _fileProvider.DeleteFile(fileData, cancellationToken);
    }
}