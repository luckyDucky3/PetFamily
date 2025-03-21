namespace PetFamily.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public FullNameDto Name { get; init; }
    public string EmailAddress { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int ExperienceYears { get; init; }


    private PetDto[] Pets { get; init; } = [];
}