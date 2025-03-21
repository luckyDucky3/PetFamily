using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;
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
            l => JsonSerializer.Serialize(l, JsonSerializerOptions.Default),
            str => JsonSerializer.Deserialize<IReadOnlyList<TValueObjects>>(str, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<TValueObjects>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()));
    }

    public static PropertyBuilder<IReadOnlyList<PetFile>> JsonValueObjectsForPetFiles(
        this PropertyBuilder<IReadOnlyList<PetFile>> builder)
    {
        return builder.HasConversion(
            file => JsonSerializer.Serialize(
                file.Select(f => new PetFileDto { PathToStorage = f.PathToStorage.Path }),
                JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<IReadOnlyList<PetFileDto>>(json, JsonSerializerOptions.Default)!
                .Select(file => new PetFile(new FilePath(file.PathToStorage))).ToList(),
            new ValueComparer<IReadOnlyList<PetFile>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    }
}