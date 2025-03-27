using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.Pets.Commands.AddPet;
using PetFamily.Domain.Models.VO;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.Pets.Commands.RemoveFilePet;

public record RemoveFilePetQuery(string FilePath, string BucketName) : IQuery;
public class RemoveFilePetHandler : IQueryHandler<string, RemoveFilePetQuery>
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<RemoveFilePetQuery> _validator;

    public RemoveFilePetHandler(
        IFileProvider fileProvider, 
        ILogger<AddPetHandler> logger, 
        IValidator<RemoveFilePetQuery> validator)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _validator = validator;
    }

    public async Task<string> Handle(RemoveFilePetQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return "";
        
        var filePath = new FilePath(query.FilePath);
        var fileData = new FileInfo(filePath, query.BucketName);
        var deleteFile = await _fileProvider.DeleteFile(fileData, cancellationToken);
        _logger.LogInformation("File {0} has been removed.", deleteFile);
        if (deleteFile.IsFailure)
            return string.Empty;
        return deleteFile.Value;
    }
}