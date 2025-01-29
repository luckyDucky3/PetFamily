using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Ids;

public class BreedId : ComparableValueObject
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }
    
    public static BreedId NewPetId() => new BreedId(Guid.NewGuid());
    public static BreedId Empty() => new BreedId(Guid.Empty);
    public static BreedId Create(Guid id) => new BreedId(id);


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator Guid(BreedId id) => id.Value;
}