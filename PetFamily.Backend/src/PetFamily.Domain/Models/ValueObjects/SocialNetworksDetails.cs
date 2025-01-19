using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

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
}