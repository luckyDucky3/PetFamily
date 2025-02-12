using FluentValidation;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Application.Volunteers.Validation;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddSocialNetworks;

public class AddSocialNetworksValidator : AbstractValidator<AddSocialNetworksRequest>
{
    public AddSocialNetworksValidator()
    {
        RuleFor(a => a.Id).NotEmpty().WithError(Errors.General.IsRequired("Id"));
    }
}

public class AddSocialNetworksDtoValidator : AbstractValidator<ListSocialNetworkDto>
{
    public AddSocialNetworksDtoValidator()
    {
        RuleForEach(l => l.SocialNetworkDtos).MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Link));
    }
}