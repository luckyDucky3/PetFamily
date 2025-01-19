using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Ids;

public class PetId : ComparableValueObject
{

    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }
    
    public static PetId NewPetId() => new PetId(Guid.NewGuid());
    public static PetId Empty() => new PetId(Guid.Empty);
    public static PetId Create(Guid id) => new PetId(id);


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator Guid(PetId id) => id.Value;
}