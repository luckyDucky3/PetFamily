using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.VO;

public class PetFile : ValueObject
{
    public FilePath PathToStorage { get; }
    //public int Size { get; }
    public PetFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PathToStorage;
    }
}