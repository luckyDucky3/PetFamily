using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Ids;

public class BreedId : ComparableValueObject
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }

    public static BreedId NewPetId() => new (Guid.NewGuid());
    public static BreedId Empty() => new (Guid.Empty);
    public static BreedId Create(Guid id) => new (id);


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator Guid(BreedId id) => id.Value;
}