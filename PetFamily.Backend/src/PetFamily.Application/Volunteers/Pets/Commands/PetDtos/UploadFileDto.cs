namespace PetFamily.Application.Volunteers.Pets.Commands.PetDtos;

public class UploadFileDto
{
    public Stream Stream { get; init; }
    public string FileName { get; init; }
}