using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class RequisitesForHelp : ValueObject
{
    public string Title { get; }
    public string DescriptionOfDonation { get; }

    private RequisitesForHelp(string title, string descriptionOfDonation)
    {
        Title = title;
        DescriptionOfDonation = descriptionOfDonation;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return DescriptionOfDonation;
    }
    public static Result<RequisitesForHelp, Error> Create(string title, string descriptionOfDonation)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<RequisitesForHelp, Error>(
                Errors.General.ValueIsInvalid("Title cannot be null or whitespace."));
        if (string.IsNullOrWhiteSpace(descriptionOfDonation))
            return Result.Failure<RequisitesForHelp, Error>(
                Errors.General.ValueIsInvalid("Description cannot be null or whitespace."));
        return Result.Success<RequisitesForHelp, Error>(new RequisitesForHelp(title, descriptionOfDonation));
    }
}