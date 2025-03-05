using FluentAssertions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;

namespace UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Single_Pet_Returns_Success()
    {
        //arrange
        var volunteer = CreateStandartVolunteer();

        var pet = CreateStandartPet();
        
        //act
        var result = volunteer.AddPet(pet);
        int firstPet = 1;
        
        //assert
        Assert.True(result.IsSuccess);
        Assert.Equal(volunteer.Pets[0].Position.Value, firstPet);
    }

    [Fact]
    public void Add_Many_Pets_Returns_Success()
    {
        //arrange
        var volunteer = CreateStandartVolunteer();
        
        var pets = Enumerable.Range(1, 5)
            .Select(_ => CreateStandartPet());
        
        var pet = CreateStandartPet();
        
        //act
        foreach (var p in pets)
            volunteer.AddPet(p);
        
        var result = volunteer.AddPet(pet);
        
        //assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[^1].Position.Value.Should().Be(volunteer.Pets.Count);
    }

    [Fact]
    public void Move_Pet_On_Next_Field_Returns_Success()
    {
        var volunteer = CreateVolunteerWithPets();
        var pet = volunteer.Pets[2];

        var result = volunteer.MovePet(pet, 2);
        
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[1].Position.Value.Should().Be(3);
        volunteer.Pets[2].Position.Value.Should().Be(2);
    }

    [Fact]
    public void Move_Pet_Across_Many_Fields_By_The_Start_Returns_Success()
    {
        var volunteer = CreateVolunteerWithPets();
        var pet = volunteer.Pets[8];
        var currentPosition = pet.Position.Value;
        int position = 3;
        var result = volunteer.MovePet(pet, position);
        //получаем список повторов (их быть не должно)
        var repeats = volunteer.Pets.GroupBy(p => p.Position).Where(g => g.Count() > 1).ToList();
        int maxSerialNumber = volunteer.Pets.Max(p => p.Position.Value);
        int minSerialNumber = volunteer.Pets.Min(p => p.Position.Value);
        
        result.IsSuccess.Should().BeTrue();
        repeats.Count.Should().Be(0);
        volunteer.Pets[currentPosition - 1].Position.Value.Should().Be(position);
        volunteer.Pets[position - 1].Position.Value.Should().Be(position + 1);
        maxSerialNumber.Should().Be(volunteer.Pets.Count());
        minSerialNumber.Should().Be(1);
    }
    
    [Fact]
    public void Move_Pet_Across_Many_Fields_By_The_End_Returns_Success()
    {
        var volunteer = CreateVolunteerWithPets();
        var pet = volunteer.Pets[1];
        var currentPosition = pet.Position.Value;
        int position = 9;
        var result = volunteer.MovePet(pet, position);
        //получаем список повторов (их быть не должно)
        var repeats = volunteer.Pets.GroupBy(p => p.Position).Where(g => g.Count() > 1).ToList();
        int maxSerialNumber = volunteer.Pets.Max(p => p.Position.Value);
        int minSerialNumber = volunteer.Pets.Min(p => p.Position.Value);
        
        result.IsSuccess.Should().BeTrue();
        repeats.Count.Should().Be(0);
        volunteer.Pets[currentPosition - 1].Position.Value.Should().Be(position);
        volunteer.Pets[currentPosition].Position.Value.Should().Be(currentPosition);
        maxSerialNumber.Should().Be(volunteer.Pets.Count());
        minSerialNumber.Should().Be(1);
    }
    [Fact]
    public void Move_Pet_In_The_End_Returns_Success()
    {
        var volunteer = CreateVolunteerWithPets();
        var pet = volunteer.Pets[6];
        var currentPosition = pet.Position.Value;
        int position = 10;
        var result = volunteer.MovePet(pet, position);
        //получаем список повторов (их быть не должно)
        var repeats = volunteer.Pets.GroupBy(p => p.Position).Where(g => g.Count() > 1).ToList();
        int maxSerialNumber = volunteer.Pets.Max(p => p.Position.Value);
        int minSerialNumber = volunteer.Pets.Min(p => p.Position.Value);
        
        result.IsSuccess.Should().BeTrue();
        repeats.Count.Should().Be(0);
        volunteer.Pets[currentPosition - 1].Position.Value.Should().Be(position);
        volunteer.Pets[currentPosition].Position.Value.Should().Be(currentPosition);
        maxSerialNumber.Should().Be(volunteer.Pets.Count);
        minSerialNumber.Should().Be(1);
    }
    [Fact]
    public void Move_Pet_In_The_Start_Returns_Success()
    {
        var volunteer = CreateVolunteerWithPets();
        var pet = volunteer.Pets[5];
        var currentPosition = pet.Position.Value;
        int position = 1;
        var result = volunteer.MovePet(pet, position);
        //получаем список повторов (их быть не должно)
        var repeats = volunteer.Pets.GroupBy(p => p.Position).Where(g => g.Count() > 1).ToList();
        int maxSerialNumber = volunteer.Pets.Max(p => p.Position.Value);
        int minSerialNumber = volunteer.Pets.Min(p => p.Position.Value);
        
        result.IsSuccess.Should().BeTrue();
        repeats.Count.Should().Be(0);
        volunteer.Pets[currentPosition - 1].Position.Value.Should().Be(position);
        volunteer.Pets[position - 1].Position.Value.Should().Be(position + 1);
        maxSerialNumber.Should().Be(volunteer.Pets.Count());
        minSerialNumber.Should().Be(1);
    }

    private Volunteer CreateVolunteerWithPets()
    {
        var volunteer = CreateStandartVolunteer();
        var pets = Enumerable.Range(1, 10)
            .Select(_ => CreateStandartPet());
        foreach (var p in pets)
            volunteer.AddPet(p);
        
        return volunteer;
    }
    private Volunteer CreateStandartVolunteer()
    {
        VolunteerId id = VolunteerId.NewVolunteerId();
        var volunteer = Volunteer.Create(id,
            FullName.Create("Full", "Name").Value,
            "description",
            EmailAddress.Create("test@test.com").Value,
            PhoneNumber.Create("8-999-999-99-99").Value, 
            0).Value;
        return volunteer;
    }

    private Pet CreateStandartPet()
    {
        PetId petId = PetId.NewPetId();
        var pet = Pet.Create(
            petId, 
            "Pet name",
            new SpeciesBreeds(
                SpecieId.Create(Guid.NewGuid()), 
                BreedId.Create(Guid.NewGuid())
            ),
            Color.Black,
            Address.Create(
                "state", 
                "city", 
                "street", 
                "homeNumber").Value
        ).Value;
        return pet;
    }
}