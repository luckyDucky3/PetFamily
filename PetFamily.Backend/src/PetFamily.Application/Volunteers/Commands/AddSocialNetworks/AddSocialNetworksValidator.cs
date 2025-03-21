using FluentValidation;
using PetFamily.Application.Dtos;
using PetFamily.Application.Validations;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Commands.AddSocialNetworks;

public class AddSocialNetworksValidator : AbstractValidator<AddSocialNetworksCommand>
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