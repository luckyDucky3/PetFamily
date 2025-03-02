using PetFamily.Domain.Models.VO;

namespace PetFamily.Application.FileProvider;

public record FileDataUpload(Stream Stream, FilePath FilePath, string BucketName);