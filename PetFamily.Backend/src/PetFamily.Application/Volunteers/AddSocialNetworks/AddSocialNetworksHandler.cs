using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddSocialNetworks;

public class AddSocialNetworksHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<AddSocialNetworksHandler> _logger;

    public AddSocialNetworksHandler(
        IVolunteersRepository volunteersRepository, 
        ILogger<AddSocialNetworksHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteer = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer == null)
            return Result.Failure<Guid, Error>(Errors.General.IsNotFound(volunteerId.Value));

        List<SocialNetwork> socialNetworks = [];
        socialNetworks.AddRange(command.SocialNetworkDtos.Select(socialNetworkDto =>
            SocialNetwork.Create(socialNetworkDto.Name, socialNetworkDto.Link).Value));

        volunteer.UpdateSocialNetworks(socialNetworks);
        
        await _volunteersRepository.Save(volunteer, cancellationToken);
        
        _logger.LogInformation("Volunteer has successfully changed social networks");
        
        return volunteer.Id.Value;
    }
}