using PetFamily.Application.Volunteers.Dto;

namespace PetFamily.Application.Volunteers.AddSocialNetworks;

public record AddSocialNetworksCommand(Guid Id, List<SocialNetworkDto> SocialNetworkDtos);