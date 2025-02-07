using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.VO;

namespace PetFamily.Infrastructure.Extensions;

public static class EfPropertyExtensions
{
    public static PropertyBuilder<DateTime> SetDateTimeKind(
        this PropertyBuilder<DateTime> builder, DateTimeKind kind)
    {
        return builder.HasConversion(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, kind));
    }

    public static PropertyBuilder<IReadOnlyList<TValueObjects>> JsonValueObjectCollectionConversion<TValueObjects>(
        this PropertyBuilder<IReadOnlyList<TValueObjects>> builder)
    {
        return builder.HasConversion(
            s => JsonSerializer.Serialize(s, JsonSerializerOptions.Default),
            str => JsonSerializer.Deserialize<IReadOnlyList<TValueObjects>>(str, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<TValueObjects>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()));
    }
}
