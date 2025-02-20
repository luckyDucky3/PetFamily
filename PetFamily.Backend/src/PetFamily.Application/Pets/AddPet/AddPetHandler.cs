using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.AddPet;

public class AddPetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(IFileProvider fileProvider, ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(FileData fileData, CancellationToken cancellationToken)
    {
        return await _fileProvider.UploadFile(fileData, cancellationToken);
    }
}