using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO
{
    public class PhoneNumber : ValueObject
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            Value = value;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        public static Result<PhoneNumber, Error> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result.Failure<PhoneNumber, Error>(
                    Errors.General.ValueIsInvalid("Phone number cannot be null or whitespace."));
            return Result.Success<PhoneNumber, Error>(new PhoneNumber(phoneNumber));
        }
        
        public static Result<PhoneNumber> Empty() => Result.Success(new PhoneNumber(""));
    }
}