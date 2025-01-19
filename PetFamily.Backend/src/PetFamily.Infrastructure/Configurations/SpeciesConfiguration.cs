using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasConversion(s => s.Value, v => SpecieId.Create(v));
        builder.HasMany(s => s.Breeds).WithOne().HasForeignKey(b=>b.SpeciesId).OnDelete(DeleteBehavior.NoAction);
        builder.Property(s => s.SpecieName).IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH).HasColumnName("specie_name");
    }
}