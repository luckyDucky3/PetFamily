using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO
{
    public class PhoneNumber : ValueObject
    {
        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = value;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        //метод для HasConversion
        public static PhoneNumber CreateWithoutCheck(string number)
        {
            return new PhoneNumber(number);
        }
        public static Result<PhoneNumber, Error> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result.Failure<PhoneNumber, Error>(
                    Errors.General.IsRequired("Phone number"));

            Regex regex = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
            if (!regex.IsMatch(phoneNumber))
                return Result.Failure<PhoneNumber, Error>(
                    Errors.General.IsInvalid("Phone number"));
            
            return Result.Success<PhoneNumber, Error>(new PhoneNumber(phoneNumber));
        }
        
        public static Result<PhoneNumber> Empty() => Result.Success(new PhoneNumber(""));
    }
}