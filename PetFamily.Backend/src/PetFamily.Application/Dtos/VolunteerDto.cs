namespace PetFamily.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public FullNameDto FullName { get; init; }
    public string EmailAddress { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int ExperienceYears { get; init; }
    
    public PetDto[] Pets { get; init; } = [];
}