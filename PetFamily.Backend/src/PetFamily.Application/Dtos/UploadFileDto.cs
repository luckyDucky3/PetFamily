namespace PetFamily.Application.Dtos;

public class UploadFileDto
{
    public Stream Stream { get; init; }
    public string FileName { get; init; }
}