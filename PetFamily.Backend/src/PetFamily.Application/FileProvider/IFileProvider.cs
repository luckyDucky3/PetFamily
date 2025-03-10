using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    public Task<Result<string, Error>> DeleteFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);

    public Task<Result<string, Error>> GetFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);
}