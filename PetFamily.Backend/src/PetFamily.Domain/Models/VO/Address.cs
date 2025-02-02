using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class Address : ValueObject
{
    public string State { get; }
    public string City { get; }
    public string Street { get; }
    public string HomeNumber { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return State;
        yield return City;
        yield return Street;
        yield return HomeNumber;
    }

    private Address(string state, string city, string street, string homeNumber)
    {
        State = state;
        City = city;
        Street = street;
        HomeNumber = homeNumber;
    }
    public static Address CreateWithoutCheck(string state, string city, string street, string homeNumber)
    {
        return new Address(state, city, street, homeNumber);
    }
    public static Result<Address, Error> Create(string state, string city, string street, string homeNumber)
    {
        if (string.IsNullOrWhiteSpace(state))
            return Result.Failure<Address, Error>(Errors.General.IsNullOrWhitespace("State"));
        
        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address, Error>(Errors.General.IsNullOrWhitespace("City"));
        
        if (string.IsNullOrWhiteSpace(street))
            return Result.Failure<Address, Error>(Errors.General.IsNullOrWhitespace("Street"));
        
        if (string.IsNullOrWhiteSpace(homeNumber))
            return Result.Failure<Address, Error>(Errors.General.IsNullOrWhitespace("HomeNumber"));
        
        return new Address(state, city, street, homeNumber);
    }
}