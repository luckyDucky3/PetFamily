using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pet");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value))
            .IsRequired();

        builder.Property(p => p.Position)
            .HasConversion(s => s.Value,
                str => Position.Create(str).Value)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_LONG_TEXT_LENGTH);

        builder.Property(p => p.Color)
            .HasConversion(new EnumToStringConverter<Color>())
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH);

        builder.Property(p => p.Weight)
            .IsRequired(false);

        builder.Property(p => p.Height)
            .IsRequired(false);

        builder.Property(p => p.PhoneNumber)
            .HasConversion(
                p => p.Value,
                s => PhoneNumber.CreateWithoutCheck(s))
            .IsRequired(false)
            .HasMaxLength(Domain.Shared.Constants.PHONE_LENGTH);

        builder.Property(p => p.IsCastrate)
            .IsRequired();

        builder.Property(p => p.IsVaccinate)
            .IsRequired();

        builder.ComplexProperty(p => p.Address, pb =>
        {
            pb.Property(p => p.State).IsRequired();
            pb.Property(p => p.City).IsRequired();
            pb.Property(p => p.Street).IsRequired();
            pb.Property(p => p.HomeNumber).IsRequired();
        });

        builder.Property(p => p.BirthDate)
            .SetDateTimeKind(DateTimeKind.Utc)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<EnumToStringConverter<Status>>();

        builder.Property(p => p.CreatedOn)
            .SetDateTimeKind(DateTimeKind.Utc)
            .IsRequired();

        builder
            .Property(p => p.InfoAboutHealth)
            .IsRequired();

        builder.OwnsOne(p => p.SpeciesBreeds, pb =>
        {
            pb.ToString();
            pb.Property(sb => sb.BreedId)
                .HasConversion(
                    b => b.Value,
                    v => BreedId.Create(v));

            pb.Property(sb => sb.SpeciesId)
                .HasConversion(
                    s => s.Value,
                    v => SpecieId.Create(v))
                .HasColumnName("species_id");
        });

        builder.Property<bool>("IsDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.Property<DateTime>("DeletionDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .SetDateTimeKind(DateTimeKind.Utc)
            .HasColumnName("deletion_date");
        
        builder.Property(p => p.Files)
            .JsonValueObjectCollectionConversion()
            .HasColumnName("files");
    }
}