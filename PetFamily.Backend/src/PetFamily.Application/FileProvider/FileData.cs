using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FileInfo FileInfo);
public record FileInfo(FilePath FilePath, string BucketName);
