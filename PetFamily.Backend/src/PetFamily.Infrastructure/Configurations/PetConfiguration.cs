using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

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
            .IsRequired()
            .HasColumnName("pet_id");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("name");

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH)
            .HasColumnName("description");

        builder.Property(p => p.Color)
            .HasConversion(new EnumToStringConverter<Color>())
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("color");

        builder.Property(p => p.Weight)
            .IsRequired(false)
            .HasColumnName("weight");

        builder.Property(p => p.Height)
            .IsRequired(false)
            .HasColumnName("height");

        builder.Property(p => p.PhoneNumber)
            .HasConversion(
                p => p.Value,
                s => PhoneNumber.CreateWithoutCheck(s))
            .IsRequired(false)
            .HasMaxLength(Constants.PHONE_LENGTH)
            .HasColumnName("phone_number");

        builder.Property(p => p.IsCastrate)
            .IsRequired()
            .HasColumnName("is_castrate");

        builder.Property(p => p.IsVaccinate)
            .IsRequired()
            .HasColumnName("is_vaccinate");

        builder.ComplexProperty(p => p.Address, pb =>
        {
            pb.Property(p => p.State).IsRequired().HasColumnName("state");
            pb.Property(p => p.City).IsRequired().HasColumnName("city");
            pb.Property(p => p.Street).IsRequired().HasColumnName("street");
            pb.Property(p => p.HomeNumber).IsRequired().HasColumnName("home_number");
        });

        builder.Property(p => p.BirthDate)
            .SetDateTimeKind(DateTimeKind.Utc)
            .IsRequired()
            .HasColumnName("birthdate");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<EnumToStringConverter<Status>>()
            .HasColumnName("status");

        builder.Property(p => p.CreatedOn)
            .SetDateTimeKind(DateTimeKind.Utc)
            .IsRequired()
            .HasColumnName("created_on");

        builder
            .Property(p => p.InfoAboutHealth)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName("info_about_health");

        builder.OwnsOne(p => p.SpeciesBreeds, pb =>
        {
            pb.ToString();
            pb.Property(sb => sb.BreedId)
                .HasConversion(
                    b => b.Value,
                    v => BreedId.Create(v))
                .HasColumnName("breed_id");

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
    }
}