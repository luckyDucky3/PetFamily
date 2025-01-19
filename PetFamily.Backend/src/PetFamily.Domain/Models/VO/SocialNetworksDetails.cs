using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public class SocialNetworksDetails : ValueObject
{
    private readonly List<SocialNetwork> _socialNetworks = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    //EF
    private SocialNetworksDetails()
    {
    }
    public SocialNetworksDetails(List<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SocialNetworks;
    }

    public Result<bool, Error> AddSocialNetwork(string socialNetworkString, string socialNetworkLink)
    {
        var response = SocialNetwork.Create(socialNetworkString, socialNetworkLink);
        if (response.IsFailure)
            return Result.Failure<bool, Error>(Errors.General.NotFound());
        var socialNetwork = response.Value;
        _socialNetworks.Add(socialNetwork);
        return Result.Success<bool, Error>(true);
    }
}