using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities;

public sealed class Pet : Entity<PetId>
{
    //EF
    private Pet()
    {
    }

    public Pet(
        PetId petId, string name, string description, SpeciesBreeds speciesBreeds, 
        Color color, Address address, string infoAboutHealth,
        double? weight, double? height, PhoneNumber phoneNumber, 
        bool isCastrate, bool isVaccinate, DateTime birthDate, Status status)
    {
        Id = petId;
        Name = name;
        Description = description;
        SpeciesBreeds = speciesBreeds;
        Color = color;
        Address = address;
        InfoAboutHealth = infoAboutHealth;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        IsCastrate = isCastrate;
        IsVaccinate = isVaccinate;
        Status = status;
        BirthDate = birthDate;
        CreatedOn = DateTime.UtcNow;
    }
    
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public SpeciesBreeds SpeciesBreeds { get; private set; } = null!;
    public Color Color { get; private set; }
    public Address Address { get; private set; } = null!;
    public string InfoAboutHealth { get; private set; } = null!;
    public double? Weight { get; private set; }
    public double? Height { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public bool IsCastrate { get; private set; }
    public bool IsVaccinate { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedOn { get; private set; }

    public Result<Pet, Error> Create(
        PetId petId, string name, string description, SpeciesBreeds speciesBreeds,
        Color color, Address address, string infoAboutHealth, double? weight,
        double? height, PhoneNumber phoneNumber, bool isCastrate, 
        bool isVaccinate, DateTime birthDate, Status status)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Pet, Error>(Errors.General.IsRequired("Name"));
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Pet, Error>(Errors.General.IsRequired("Description"));
        
        if (string.IsNullOrWhiteSpace(infoAboutHealth))
            return Result.Failure<Pet, Error>(Errors.General.IsRequired("Info about health"));
        
        if (weight < 0)
            return Result.Failure<Pet, Error>(Errors.General.IsInvalid("Weight"));
        
        if (height < 0)
            return Result.Failure<Pet, Error>(Errors.General.IsInvalid("Height"));
        
        if (BirthDate < DateTime.MinValue || BirthDate > DateTime.Now)
            return Result.Failure<Pet, Error>(Errors.General.IsInvalid("Birthdate"));
        
        Pet pet = new Pet(petId, name, description, speciesBreeds, 
            color, address, infoAboutHealth, weight, 
            height, phoneNumber, isCastrate, isVaccinate, 
            birthDate, status);
        
        return Result.Success<Pet, Error>(pet);
    }
}