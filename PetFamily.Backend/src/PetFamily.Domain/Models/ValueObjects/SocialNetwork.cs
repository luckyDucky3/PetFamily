using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models.ValueObjects;

public class SocialNetwork : ValueObject
{
    public string Name { get; }
    public string Link { get; }

    public SocialNetwork(string name, string link)
    {
        Name = name;
        Link = link;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Link;
    }
}