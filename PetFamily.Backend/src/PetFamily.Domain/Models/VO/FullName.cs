using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public sealed class FullName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? Patronymic { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    private FullName(string firstName, string lastName, string patronymic) : this(firstName, lastName)
    {
        Patronymic = patronymic;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Patronymic;
    }

    public static Result<FullName, Error> Create(string firstName, string lastName, string? patronymic = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FullName, Error>(
                Errors.General.IsInvalid("First name"));
        
        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<FullName, Error>(
                Errors.General.IsInvalid("Last name"));
        
        if (string.IsNullOrWhiteSpace(patronymic))
            return Result.Success<FullName, Error>(new FullName(firstName, lastName));
        
        return Result.Success<FullName, Error>(new FullName(firstName, lastName, patronymic));
    }
}