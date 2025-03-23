using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.Pets.Commands.PetDtos;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Ids;

namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    public int Position { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Color Color { get; init; }
    //public AddressDto Address { get; init; }
    public string InfoAboutHealth { get; init; } = string.Empty;
    public double? Weight { get; init; }
    public double? Height { get; init; }
    public string? PhoneNumber { get; init; } = string.Empty;
    public bool IsCastrate { get; init; }
    public bool IsVaccinate { get; init; }
    public DateTime BirthDate { get; init; }
    public Status Status { get; init; }
    public DateTime CreatedOn { get; init; }
    public PetFileDto[] Files { get; set; }
    public Guid SpecieId { get; init; }
    public Guid BreedId { get; init; }
    
    public Guid VolunteerId { get; init; }
}

public class PetFileDto
{
    public string PathToStorage { get; set; } = string.Empty;
    public int Size { get; set; }
}