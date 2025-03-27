using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.VO;

public class PetFile : ValueObject
{
    public FilePath PathToStorage { get; }
    public int Size { get; } = 0;
    public bool IsGeneral { get; }
    [JsonConstructor]
    public PetFile(FilePath pathToStorage, bool isGeneral)
    {
        PathToStorage = pathToStorage;
        IsGeneral = isGeneral;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PathToStorage;
        yield return Size;
        yield return IsGeneral;
    }
}