using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class FilePath : ValueObject
{
    private FilePath(string path)
    {
        PathToStorage = path;
    }
    public string PathToStorage { get; }

    public static Result<FilePath, Error> Create(Guid path, string extension)
    {
        //валидация на доступные расширения файлов
        var fullPath = path + "." + extension;
        return new FilePath(fullPath);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PathToStorage;
    }
}