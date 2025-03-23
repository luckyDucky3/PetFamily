using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Entities.Volunteer;

public sealed class Pet : SoftDeletableEntity<PetId>
{
    private Pet(PetId id) : base(id)
    {
    }

    public Position Position { get; private set; }
    public Volunteer Volunteer { get; private set; } = null!;

    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public SpeciesBreeds? SpeciesBreeds { get; private set; }
    public Color Color { get; private set; }
    public Address Address { get; private set; } = null!;
    public string InfoAboutHealth { get; private set; } = null!;
    public double? Weight { get; private set; }
    public double? Height { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public bool IsCastrate { get; private set; }
    public bool IsVaccinate { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Status Status { get; private set; }
    public DateTime CreatedOn { get; private set; }
    private List<PetFile> _files = [];
    public IReadOnlyList<PetFile> Files => _files;

    private Pet(
        PetId petId, 
        string name,
        SpeciesBreeds speciesBreeds,
        Color color, 
        Address address, 
        string description, 
        string infoAboutHealth, 
        double? weight,
        double? height, 
        PhoneNumber? phoneNumber, 
        bool isCastrate,
        bool isVaccinate,
        DateTime birthDate,
        Status status, 
        IEnumerable<PetFile>? files = null) : this(petId)
    {
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
        if (files != null)
            _files = files.ToList();
    }

    public static Result<Pet, Error> Create(
        PetId petId, 
        string name, 
        SpeciesBreeds speciesBreeds,
        Color color, Address address,
        string description = "", 
        string infoAboutHealth = "",
        double? weight = null, 
        double? height = null, 
        PhoneNumber? phoneNumber = null,
        bool isCastrate = false, 
        bool isVaccinate = false, 
        DateTime birthDate = default,
        Status status = Status.FindHome, 
        IEnumerable<PetFile>? files = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Pet, Error>(Errors.General.IsRequired("Name"));

        if (weight is < 0)
            return Result.Failure<Pet, Error>(Errors.General.IsInvalid("Weight"));

        if (height is < 0)
            return Result.Failure<Pet, Error>(Errors.General.IsInvalid("Height"));

        if (birthDate < DateTime.MinValue || birthDate > DateTime.Now)
            return Result.Failure<Pet, Error>(Errors.General.IsInvalid("Birthdate"));
        
        Pet pet = new Pet(petId, name, speciesBreeds,
            color, address, description, infoAboutHealth, weight,
            height, phoneNumber, isCastrate, isVaccinate,
            birthDate, status, files);

        return pet;
    }

    public void UpdateMainInfo(
        string name, 
        SpeciesBreeds speciesBreeds,
        Color color, Address address,
        string description = "", 
        string infoAboutHealth = "",
        double? weight = null, 
        double? height = null, 
        PhoneNumber? phoneNumber = null,
        bool isCastrate = false, 
        bool isVaccinate = false, 
        DateTime birthDate = default,
        Status status = Status.FindHome)
    {
        Name = name;
        SpeciesBreeds = speciesBreeds;
        Color = color;
        Address = address;
        Description = description;
        InfoAboutHealth = infoAboutHealth;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        IsCastrate = isCastrate;
        IsVaccinate = isVaccinate;
        BirthDate = birthDate;
        Status = status;
    }
    
    public void UpdateStatus(Status status)
    => Status = status;
    
    public void SetPosition(Position position)
    {
        Position = position;
    }

    public void UpdateFiles(List<PetFile> petFiles)
        => _files = petFiles;
}