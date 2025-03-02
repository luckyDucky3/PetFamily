using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const string MAX_DEGREE = "Minio:MaxDegreeOfParallelism";
    
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;
    private static IConfiguration _configuration = null!;
    private static readonly int MaxDegreeOfParallelism = _configuration.GetValue<int>(MAX_DEGREE);

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileDataUpload> filesData,
        CancellationToken cancellationToken = default)
    {
        using SemaphoreSlim semaphoreSlim = new(MaxDegreeOfParallelism);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketNotExistCreateBucket(filesList, cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var results = pathsResult.Select(p => p.Value).ToList();
            
            _logger.LogInformation("Uploaded fies: {files}", results.Select(r => r.PathToStorage));
            
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Result.Failure<IReadOnlyList<FilePath>, Error>(
                Error.Failure("file.upload",
                "Fail to upload files in minio"));
        }
    }

    public async Task<Result<string, Error>> DeleteFile(
        FileDataRemove fileDataRemove,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileDataRemove.BucketName)
                .WithObject(fileDataRemove.ObjectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            return Result.Success<string, Error>(fileDataRemove.ObjectName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to delete file in minio");
            return Result.Failure<string, Error>(Error.Failure("fail.delete", "fail to delete file in minio"));
        }
    }

    public async Task<Result<string, Error>> GetFile(
        FileDataGet fileDataGet,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileDataGet.BucketName)
                .WithObject(fileDataGet.ObjectName)
                .WithExpiry(60 * 60 * 24);
            var result = await _minioClient.PresignedGetObjectAsync(getObjectArgs);
            return Result.Success<string, Error>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to get file path in minio");
            return Result.Failure<string, Error>(Error.Failure("fail.get", "fail to get file path in minio"));
        }
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileDataUpload fileDataUpload,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileDataUpload.BucketName)
            .WithStreamData(fileDataUpload.Stream)
            .WithObjectSize(fileDataUpload.Stream.Length)
            .WithObject(fileDataUpload.FilePath.PathToStorage);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return fileDataUpload.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileDataUpload.FilePath.PathToStorage,
                fileDataUpload.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task IfBucketNotExistCreateBucket(IEnumerable<FileDataUpload> filesData,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketNames = [..filesData.Select(f => f.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExist =
                await _minioClient.BucketExistsAsync(
                    new BucketExistsArgs().WithBucket(bucketName),
                    cancellationToken);
            if (!bucketExist)
            {
                var bucket = new MakeBucketArgs().WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(bucket, cancellationToken);
            }
        }
    }
}