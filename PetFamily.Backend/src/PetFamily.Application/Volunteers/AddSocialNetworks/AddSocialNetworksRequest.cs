using FluentValidation;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddSocialNetworks;

public record AddSocialNetworksRequest(Guid Id, ListSocialNetworkDto SocialNetworkDtos);