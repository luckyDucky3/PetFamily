using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class FilePath : ValueObject
{
    //EF
    [JsonConstructor]
    public FilePath(string path)
    {
        Path = path;
    }
    public string Path { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        //валидация на доступные расширения файлов
        var fullPath = path + extension;
        return new FilePath(fullPath);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Path;
    }
}