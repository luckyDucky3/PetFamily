using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class RequisitesForHelp : ValueObject
{
    public string value { get; }

    public RequisitesForHelp(string value)
    {
        value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return value;
    }
}