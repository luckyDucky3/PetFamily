using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.VO;
using PetFamily.Infrastructure.Extensions;


namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pet");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.BirthDate).SetDateTimeKind(DateTimeKind.Utc);
        builder.Property(p => p.Color).HasConversion(new EnumToStringConverter<Color>());
        builder.Property(p => p.Status).HasConversion(new EnumToStringConverter<Status>());
        builder.Property(p => p.Files)
            .HasConversion(
                files => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IReadOnlyList<PetFile>>(json, JsonSerializerOptions.Default)!
                    .Select(f => new PetFileDto
                    {
                        PathToStorage = f.PathToStorage.Path
                    }).ToArray());
    }
}