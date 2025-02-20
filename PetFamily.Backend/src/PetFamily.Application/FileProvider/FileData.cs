namespace PetFamily.Application.FileProvider;

public record FileData(Stream FileStream, string BucketName, string ObjectName);

public record FileDataRemove(string BucketName, string ObjectName);

public record FileDataGet(string BucketName, string ObjectName);