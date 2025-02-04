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
        CreateVolunteerCommand createVolunteerCommand, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullNameResult = FullName.Create(
            createVolunteerCommand.FirstName, createVolunteerCommand.LastName, createVolunteerCommand.Patronymic);
        if (fullNameResult.IsFailure)
            return Result.Failure<Guid, Error>(fullNameResult.Error);
        
        var phoneNumberResult = PhoneNumber.Create(createVolunteerCommand.PhoneNumber);
        if (phoneNumberResult.IsFailure)
            return Result.Failure<Guid, Error>(phoneNumberResult.Error);
        
        var emailAddressResult = EmailAddress.Create(createVolunteerCommand.EmailAddress);
        if (emailAddressResult.IsFailure)
            return Result.Failure<Guid, Error>(emailAddressResult.Error);
        
        var experienceYearsResult = ExperienceYearsValidation(createVolunteerCommand.ExperienceYears);
        if (experienceYearsResult.IsFailure)
            return Result.Failure<Guid, Error>(experienceYearsResult.Error);

        var descriptionResult = DescriptionValidation(createVolunteerCommand.Description);
        if (descriptionResult.IsFailure)
            return Result.Failure<Guid, Error>(descriptionResult.Error);

        List<SocialNetwork> socialNetworks = new List<SocialNetwork>();
        foreach (var socialNetwork in createVolunteerCommand.SocialNetworks)
        {
            var socialNetworkResult = SocialNetwork.Create(
                socialNetwork.Name, socialNetwork.Link);
            
            if (socialNetworkResult.IsFailure)
                return Result.Failure<Guid, Error>(socialNetworkResult.Error);
            
            socialNetworks.Add(socialNetworkResult.Value);
        }
        
        List<RequisitesForHelp> requisitesForHelp = new List<RequisitesForHelp>();
        foreach (var requisiteForHelp in createVolunteerCommand.RequisitesForHelp)
        {
            var requisitesForHelpResult = RequisitesForHelp.Create(
                requisiteForHelp.Title, requisiteForHelp.Description);
            
            if (requisitesForHelpResult.IsFailure)
                return Result.Failure<Guid, Error>(requisitesForHelpResult.Error);
            
            requisitesForHelp.Add(requisitesForHelpResult.Value);
        }
        
        var volunteerResult = Volunteer.Create(
            volunteerId, fullNameResult.Value, 
            emailAddressResult.Value, descriptionResult.Value, 
            phoneNumberResult.Value, experienceYearsResult.Value);
        
        if (volunteerResult.IsFailure)
            return Result.Failure<Guid, Error>(volunteerResult.Error);
        
        if (socialNetworks.Any())
            volunteerResult.Value.CreateSocialNetworks(socialNetworks);
        
        if (requisitesForHelp.Any())
            volunteerResult.Value.CreateRequisitesForHelp(requisitesForHelp);
        
        Guid vId = await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
        return Result.Success<Guid, Error>(vId);

        //ниже функции валидации полей примиивных типов данных
        static Result<int, Error> ExperienceYearsValidation(int years)
        {
            if (years < 0 || years > Constants.MAX_EXP_YEARS)
                return Result.Failure<int, Error>(Errors.General.IsInvalid("Experience years"));
            return Result.Success<int, Error>(years);
        }

        static Result<string, Error> DescriptionValidation(string description)
        {
            if (description.Length > Constants.MAX_LONG_TEXT_LENGTH)
                return Result.Failure<string, Error>(Errors.General.IsInvalidLength("Description"));
            return Result.Success<string, Error>(description);
        }
    }
}