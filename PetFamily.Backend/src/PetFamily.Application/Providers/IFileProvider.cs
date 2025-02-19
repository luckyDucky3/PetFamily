using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Providers;

public interface IFileProvider
{
    public Task<Result<string, Error>> UploadFile(
        FileData fileData, CancellationToken cancellationToken = default);

    public Task<Result<string, Error>> DeleteFile(
        FileDataRemove fileDataRemove,
        CancellationToken cancellationToken = default);

    public Task<Result<string, Error>> GetFile(
        FileDataGet fileDataGet,
        CancellationToken cancellationToken = default);
}