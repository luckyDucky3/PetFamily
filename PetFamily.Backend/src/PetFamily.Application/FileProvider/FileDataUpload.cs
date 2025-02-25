using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.FileProvider;

public record FileDataUpload(Stream Stream, FilePath FilePath, string BucketName);

public record FileDataRemove(string BucketName, string ObjectName);

public record FileDataGet(string BucketName, string ObjectName);