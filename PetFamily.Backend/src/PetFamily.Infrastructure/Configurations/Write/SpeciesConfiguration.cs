using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Specie>
{
    public void Configure(EntityTypeBuilder<Specie> builder)
    {
        builder.ToTable("species");
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(
                s => s.Value, 
                v => SpecieId.Create(v));
        
        builder.Property(s => s.SpecieName)
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("specie_name");
    }
}