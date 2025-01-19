using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class EmailAdress : ValueObject
{
    public string Value { get; }

    public EmailAdress(string value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}