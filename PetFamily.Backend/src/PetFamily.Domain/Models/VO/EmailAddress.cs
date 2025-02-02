using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public sealed class EmailAddress : ValueObject
{
    public string Value { get; }

    private EmailAddress(string value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    //метод для HasConversion
    public static EmailAddress CreateWithoutCheck(string emailAddress)
    {
        return new EmailAddress(emailAddress);
    }
    
    public static Result<EmailAddress, Error> Create(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            return Result.Failure<EmailAddress, Error>(
                Errors.General.IsNullOrWhitespace("Email address"));
        
        Regex regex = new Regex(@"^\S+@\S+\.\S+$");
        if (!regex.IsMatch(emailAddress))
            return Result.Failure<EmailAddress, Error>(
                Errors.General.IsInvalid("Email address"));
        
        return Result.Success<EmailAddress, Error>(new EmailAddress(emailAddress));
    }

    public static Result<EmailAddress> Empty() => Result.Success(new EmailAddress(""));
}