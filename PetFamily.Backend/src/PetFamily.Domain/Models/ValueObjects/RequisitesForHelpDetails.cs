using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class RequisitesForHelpDetails : ValueObject
{
    private readonly List<RequisitesForHelp> _requisitesForHelp = [];
    public IReadOnlyList<RequisitesForHelp> ListRequisitesForHelp => _requisitesForHelp;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ListRequisitesForHelp;
    }
}