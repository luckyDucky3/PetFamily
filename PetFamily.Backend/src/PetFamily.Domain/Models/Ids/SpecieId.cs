using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Ids;

public class SpecieId : ComparableValueObject
{
    public Guid Value { get; }

    private SpecieId(Guid value)
    {
        Value = value;
    }
    
    public static SpecieId NewPetId => new SpecieId(Guid.NewGuid());
    public static SpecieId Empty => new SpecieId(Guid.Empty);
    public static SpecieId Create(Guid id) => new SpecieId(id);


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}