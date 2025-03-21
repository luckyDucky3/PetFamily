using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Models.VO;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Queries.GetPet;

public record GetPetQuery(string FilePath, string BucketName) : IQuery;

public class GetPetHandler : IQueryHandler<string, GetPetQuery>
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<GetPetHandler> _logger;

    public GetPetHandler(IFileProvider fileProvider, ILogger<GetPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<string> Handle(
            GetPetQuery query,
            CancellationToken cancellationToken) 
    {
        var filePath = new FilePath(query.FilePath);

        FileInfo fileInfo = new FileInfo(filePath, query.BucketName);
        var fileResult = await _fileProvider.GetFile(fileInfo, cancellationToken);
        if (fileResult.IsFailure)
            return string.Empty;
        
        _logger.LogInformation("Get pet: {0}", fileResult.Value);
        return fileResult.Value;
    }
}