using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class RequisitesForHelp : ValueObject
{
    public string RequisiteName { get; }

    public RequisitesForHelp(string requisiteName)
    {
        RequisiteName = requisiteName;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RequisiteName;
    }
}