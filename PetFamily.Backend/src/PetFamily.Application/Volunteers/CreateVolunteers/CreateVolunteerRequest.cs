namespace PetFamily.Application.Volunteers.CreateVolunteers;


public record CreateVolunteerRequest(
    string FirstName, string LastName,
    string Description, string PhoneNumber,
    string EmailAddress, int ExperienceYears);
