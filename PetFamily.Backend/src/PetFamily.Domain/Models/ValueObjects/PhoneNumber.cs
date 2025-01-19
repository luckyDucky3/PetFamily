using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects
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
    }
}