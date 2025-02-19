using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await BucketExistCheck(fileData.BucketName, cancellationToken);

            var path = Guid.NewGuid();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.BucketName)
                .WithStreamData(fileData.FileStream)
                .WithObjectSize(fileData.FileStream.Length)
                .WithObject(path.ToString());

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
            return Result.Success<string, Error>(result.ObjectName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to upload file in minio");
            return Result.Failure<string, Error>(Error.Failure("fail.upload", "fail to upload file in minio"));
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
                .WithExpiry(60 * 60 *24);
            var result = await _minioClient.PresignedGetObjectAsync(getObjectArgs);
            return Result.Success<string, Error>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to get file path in minio");
            return Result.Failure<string, Error>(Error.Failure("fail.get", "fail to get file path in minio"));
        }
    }

    public async Task BucketExistCheck(string bucketName, CancellationToken cancellationToken = default)
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