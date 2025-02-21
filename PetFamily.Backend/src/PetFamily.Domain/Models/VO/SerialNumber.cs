using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class SerialNumber : ValueObject
{
    public int Value { get; }
    private SerialNumber(int value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<SerialNumber, Error> Create(int number)
    {
        if (number <= 0)
            return Result.Failure<SerialNumber, Error>(Errors.General.IsInvalid("Serial number"));
        
        return Result.Success<SerialNumber, Error>(new SerialNumber(number));
    }
}