using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Number { get; }

        public PhoneNumber(string number)
        {
            Number = number;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }
    }
}