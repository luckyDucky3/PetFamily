using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Pets.Commands.AddPet;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Commands.RemovePet;

public record RemovePetQuery(string FilePath, string BucketName) : IQuery;
public class RemovePetHandler : IQueryHandler<string, RemovePetQuery>
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;

    public RemovePetHandler(IFileProvider fileProvider, ILogger<AddPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<string> Handle(RemovePetQuery query, CancellationToken cancellationToken)
    {
        var filePath = new FilePath(query.FilePath);
        var fileData = new FileInfo(filePath, query.BucketName);
        var deleteFile = await _fileProvider.DeleteFile(fileData, cancellationToken);
        _logger.LogInformation("File {0} has been removed.", deleteFile);
        if (deleteFile.IsFailure)
            return string.Empty;
        return deleteFile.Value;
    }
}