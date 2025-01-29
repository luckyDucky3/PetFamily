using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pet");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(id => id.Value, value => PetId.Create(value)).IsRequired().HasColumnName("pet_id");
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH).HasColumnName("name");
        builder.Property(p => p.Description).IsRequired().HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH).HasColumnName("description");
        builder.Property(p => p.Color).IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH).HasColumnName("color");
        builder.Property(p => p.Weight).IsRequired(false).HasColumnName("weight");
        builder.Property(p => p.Height).IsRequired(false).HasColumnName("height");
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(Constants.PHONE_LENGTH).HasColumnName("phone_number");
        builder.Property(p => p.IsCastrate).IsRequired().HasColumnName("is_castrate");
        builder.Property(p => p.IsVaccinate).IsRequired().HasColumnName("is_vaccinate");
        builder.Property(p => p.BirthDate).IsRequired(false).HasColumnName("birthdate");
        builder.Property(p => p.Status).IsRequired().HasColumnName("status");
        builder.Property(p => p.CreatedOn).IsRequired().HasColumnName("created_on");
        builder.Property(p=>p.InfoAboutHealth).IsRequired().HasColumnType("text").HasColumnName("info_about_health");
        
        builder.OwnsOne(p => p.SpeciesBreeds, pb =>
        {
            pb.ToString();
            pb.Property(sb => sb.BreedId).HasConversion(b => b.Value, v=> BreedId.Create(v)).HasColumnName("breed_id");
            pb.Property(sb => sb.SpeciesId).HasConversion(s => s.Value, v=> SpecieId.Create(v)).HasColumnName("species_id");
        });
    }
}