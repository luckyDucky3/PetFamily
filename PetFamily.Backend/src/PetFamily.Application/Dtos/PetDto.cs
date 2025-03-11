using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.Pets.Commands.PetDtos;
using PetFamily.Domain.Enums;

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
    public string PhoneNumber { get; init; } = string.Empty;
    public bool IsCastrate { get; init; }
    public bool IsVaccinate { get; init; }
    public DateTime BirthDate { get; init; }
    public Status Status { get; init; }
    public DateTime CreatedOn { get; init; }
    
    [Column("pet_volunteer_id")]
    public Guid VolunteerId { get; init; }
}