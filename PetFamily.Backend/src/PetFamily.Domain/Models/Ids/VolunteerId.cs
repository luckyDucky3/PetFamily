using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.Ids;

public class VolunteerId : ComparableValueObject
{
    public Guid Value { get; }

    private VolunteerId(Guid value)
    {
        Value = value;
    }
    
    public static VolunteerId NewVolunteerId() => new VolunteerId(Guid.NewGuid());
    public static VolunteerId Empty() => new VolunteerId(Guid.Empty);
    public static VolunteerId Create(Guid id) => new VolunteerId(id);


    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator Guid(VolunteerId volunteerId)
    {
        ArgumentNullException.ThrowIfNull(volunteerId);
        return volunteerId.Value;
    }
}