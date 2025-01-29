using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.VO;

public class RequisitesForHelpDetails : ValueObject
{
    private readonly List<RequisitesForHelp> _requisitesForHelp = [];
    public IReadOnlyList<RequisitesForHelp> ListRequisitesForHelp => _requisitesForHelp;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ListRequisitesForHelp;
    }
    public RequisitesForHelpDetails()
    {
    }

    public Result<bool> AddRequisite(RequisitesForHelp requisite)
    {
        _requisitesForHelp.Add(requisite);
        return Result.Success(true);
    }
}