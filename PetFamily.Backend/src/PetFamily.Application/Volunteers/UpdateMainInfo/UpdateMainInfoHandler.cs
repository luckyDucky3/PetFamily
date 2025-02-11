using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoCommand updateMainInfo,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(updateMainInfo.Id);
        var volunteerResult = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteerResult == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound());

        var fullName = FullName.Create(
            updateMainInfo.Fullname.FirstName, 
            updateMainInfo.Fullname.LastName,
            updateMainInfo.Fullname.Patronymic).Value;
        
        var emailAddress = EmailAddress.Create(updateMainInfo.EmailAddress).Value;
        
        var phoneNumber = PhoneNumber.Create(updateMainInfo.PhoneNumber).Value;
        
        var description = updateMainInfo.Description;
        
        int experienceYears = updateMainInfo.ExperienceYears;
        
        volunteerResult.UpdateMainInfo(fullName, description, emailAddress, phoneNumber, experienceYears);
        
        var result = await _volunteersRepository.Save(volunteerResult, cancellationToken);
        
        _logger.LogInformation("Volunteer has been successfully updated");
        
        return Result.Success<Guid, Error>(result);
    }
}