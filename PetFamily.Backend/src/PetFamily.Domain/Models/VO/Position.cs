using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class Position : ValueObject
{
    public int Value { get; }
    private Position(int value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Result<Position, Error> Create(int number)
    {
        if (number < 1)
            return Result.Failure<Position, Error>(Errors.General.IsInvalid("Serial number"));
        
        return Result.Success<Position, Error>(new Position(number));
    }
    
    public static void Forward(Pet pet)
        => pet.SetPosition(Position.Create(pet.Position.Value + 1).Value);
    
    public static void Backward(Pet pet)
        => pet.SetPosition(Position.Create(pet.Position.Value - 1).Value);
    
    public static void SetNewPosition(Pet pet, int newPosition)
        => pet.SetPosition(Position.Create(newPosition).Value);
}