using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public sealed class HelpRequisite : ValueObject
{
    public string Title { get; }
    public string DescriptionOfDonation { get; }

    public HelpRequisite() { }
    private HelpRequisite(string title, string descriptionOfDonation)
    {
        Title = title;
        DescriptionOfDonation = descriptionOfDonation;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return DescriptionOfDonation;
    }
    public static Result<HelpRequisite, Error> Create(string title, string descriptionOfDonation)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<HelpRequisite, Error>(
                Errors.General.IsInvalid("Title"));
        
        if (string.IsNullOrWhiteSpace(descriptionOfDonation))
            return Result.Failure<HelpRequisite, Error>(
                Errors.General.IsInvalid("Description"));
        
        return Result.Success<HelpRequisite, Error>(
            new HelpRequisite(title, descriptionOfDonation));
    }
}