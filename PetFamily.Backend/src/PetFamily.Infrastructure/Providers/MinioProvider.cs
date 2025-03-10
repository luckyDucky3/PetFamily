using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.MessageQueues;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const string MAX_DEGREE = "Minio:MaxDegreeOfParallelism";
    
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly int _maxDegreeOfParallelism;

    public MinioProvider(
        IMinioClient minioClient, 
        ILogger<MinioProvider> logger,
        IConfiguration configuration,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue)
    {
        _minioClient = minioClient;
        _logger = logger;
        _messageQueue = messageQueue;
        _maxDegreeOfParallelism = configuration.GetValue<int>(MAX_DEGREE);
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        using SemaphoreSlim semaphoreSlim = new(_maxDegreeOfParallelism);
        var dataList = filesData.ToList();
        var filesList = dataList.ToList();

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
            await _messageQueue.WriteAsync(dataList.Select(f => f.FileInfo), cancellationToken);
            
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Result.Failure<IReadOnlyList<FilePath>, Error>(
                Error.Failure("file.upload",
                "Fail to upload files in minio"));
        }
    }

    public async Task<Result<string, Error>> DeleteFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var statArgs = new StatObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.PathToStorage);
            if (statArgs == null)
                return Result.Success<string, Error>(fileInfo.FilePath.PathToStorage);

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.PathToStorage);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            return Result.Success<string, Error>(fileInfo.FilePath.PathToStorage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to delete file in minio");
            return Result.Failure<string, Error>(Error.Failure("fail.delete", "fail to delete file in minio"));
        }
    }

    public async Task<Result<string, Error>> GetFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.FilePath.PathToStorage)
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
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.FileInfo.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FileInfo.FilePath.PathToStorage);

        try
        {
            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return fileData.FileInfo.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FileInfo.FilePath.PathToStorage,
                fileData.FileInfo.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
    
    private async Task IfBucketNotExistCreateBucket(IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        HashSet<string> bucketNames = [..filesData.Select(f => f.FileInfo.BucketName)];

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