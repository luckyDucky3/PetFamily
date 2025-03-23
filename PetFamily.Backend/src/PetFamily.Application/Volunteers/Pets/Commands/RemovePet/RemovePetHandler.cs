using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
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
    private readonly IValidator<RemovePetQuery> _validator;

    public RemovePetHandler(
        IFileProvider fileProvider, 
        ILogger<AddPetHandler> logger, 
        IValidator<RemovePetQuery> validator)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _validator = validator;
    }

    public async Task<string> Handle(RemovePetQuery query, CancellationToken cancellationToken)
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