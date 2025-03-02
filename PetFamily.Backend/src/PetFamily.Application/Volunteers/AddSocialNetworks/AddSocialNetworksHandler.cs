using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers._Dto;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddSocialNetworks;

public record AddSocialNetworksCommand(Guid Id, List<SocialNetworkDto> SocialNetworkDtos);

public class AddSocialNetworksHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddSocialNetworksHandler> _logger;
    private readonly IValidator<AddSocialNetworksCommand> _validator;

    public AddSocialNetworksHandler(
        IVolunteersRepository volunteersRepository, 
        ILogger<AddSocialNetworksHandler> logger,
        IValidator<AddSocialNetworksCommand> validator)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Errors.General.IsNotFound(volunteerId.Value).ToErrorList();

        List<SocialNetwork> socialNetworks = [];
        socialNetworks.AddRange(command.SocialNetworkDtos.Select(socialNetworkDto =>
            SocialNetwork.Create(socialNetworkDto.Name, socialNetworkDto.Link).Value));

        volunteer.UpdateSocialNetworks(socialNetworks);
        
        await _volunteersRepository.Save(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer has successfully changed social networks");
        
        return volunteer.Id.Value;
    }
}