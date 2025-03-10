using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Pets.AddPet;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.RemovePet;

public class RemovePetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;

    public RemovePetHandler(IFileProvider fileProvider, ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(FileInfo fileData, CancellationToken cancellationToken)
    {
        return await _fileProvider.DeleteFile(fileData, cancellationToken);
    }
}