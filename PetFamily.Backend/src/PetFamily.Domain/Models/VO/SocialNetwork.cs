using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.VO;

public sealed class SocialNetwork : ValueObject
{
    public string Name { get; }
    public string Link { get; }

    private SocialNetwork(string name, string link)
    {
        Name = name;
        Link = link;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Link;
    }
    public static Result<SocialNetwork, Error> Create(string name, string link)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<SocialNetwork, Error>(
                Errors.General.IsInvalid("Name"));
        
        if (string.IsNullOrWhiteSpace(link))
            return Result.Failure<SocialNetwork, Error>(
                Errors.General.IsInvalid("Link"));
        
        return Result.Success<SocialNetwork, Error>(new SocialNetwork(name, link));
    }
}