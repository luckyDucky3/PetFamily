using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetPet;

public class GetPetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<GetPetHandler> _logger;

    public GetPetHandler(IFileProvider fileProvider, ILogger<GetPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Handle(FileInfo fileInfo, CancellationToken cancellationToken)
    {
        return await _fileProvider.GetFile(fileInfo, cancellationToken);
    }
}