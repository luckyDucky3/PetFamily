using CSharpFunctionalExtensions;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteers;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest createVolunteerRequest, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullNameResult = FullName.Create(createVolunteerRequest.FirstName, createVolunteerRequest.LastName);
        if (fullNameResult.IsFailure)
            return Result.Failure<Guid, Error>(fullNameResult.Error);

        var volunteerResult = Volunteer.Create(volunteerId, fullNameResult.Value);
        if (volunteerResult.IsFailure)
            return Result.Failure<Guid, Error>(volunteerResult.Error);
        
        Guid vId = await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
        return Result.Success<Guid, Error>(vId);
    }
}