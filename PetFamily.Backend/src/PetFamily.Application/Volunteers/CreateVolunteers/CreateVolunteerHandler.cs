using System.Text;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Entities.Volunteer;
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

        var experienceYears = createVolunteerCommand.ExperienceYears;

        var description = createVolunteerCommand.Description;

        var fullNameResult = FullName.Create(
            createVolunteerCommand.FirstName,
            createVolunteerCommand.LastName,
            createVolunteerCommand.Patronymic).Value;

        var phoneNumber = PhoneNumber.Create(
            createVolunteerCommand.PhoneNumber).Value;

        var emailAddress = EmailAddress.Create(
            createVolunteerCommand.EmailAddress).Value;

        List<SocialNetwork> socialNetworks = [];
        socialNetworks.AddRange(
            createVolunteerCommand.SocialNetworks.Select(
                socialNetwork => SocialNetwork.Create(
                    socialNetwork.Name, socialNetwork.Link).Value));

        List<RequisitesForHelp> requisitesForHelp = [];
        requisitesForHelp.AddRange(
            createVolunteerCommand.RequisitesForHelp.Select(
                requisiteForHelp => RequisitesForHelp.Create(
                    requisiteForHelp.Title, requisiteForHelp.Description).Value));

        var volunteerResult = Volunteer.Create(
            volunteerId, fullNameResult,
            emailAddress, description,
            phoneNumber, experienceYears);

        if (volunteerResult.IsFailure)
            return Result.Failure<Guid, Error>(volunteerResult.Error);

        if (socialNetworks.Any())
            volunteerResult.Value.CreateSocialNetworks(socialNetworks);

        if (requisitesForHelp.Any())
            volunteerResult.Value.CreateRequisitesForHelp(requisitesForHelp);

        Guid vId = await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
        return Result.Success<Guid, Error>(vId);
        
    }
}