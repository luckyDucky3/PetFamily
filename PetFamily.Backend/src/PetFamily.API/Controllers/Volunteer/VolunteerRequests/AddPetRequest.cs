using PetFamily.Application.Pets.AddPet;
using PetFamily.Domain.Enums;

namespace PetFamily.API.Controllers.Volunteer.VolunteerRequests;

public record AddPetRequest(
    string PetName,
    Guid SpecieId,
    Guid BreedId,
    Color Color,
    AddressDto AddressDto,
    IFormFileCollection Files);

// public record AddHelpRequisitesRequest(Guid Id, ListHelpRequisiteDto HelpRequisiteDtos);
//
// public record AddSocialNetworksRequest(Guid Id, ListSocialNetworkDto SocialNetworkDtos);

// public record HardDeleteVolunteerRequest(Guid Id);
//
// public record SoftDeleteVolunteerRequest(Guid Id);
//
// public record UpdateMainInfoRequest(Guid Id, UpdateMainInfoDto Dto);


    