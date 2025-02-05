using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breed");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Id)
            .HasConversion(
                b => b.Value, 
                v => BreedId.Create(v))
            .IsRequired()
            .HasColumnName("breed_id");
        
        builder.Property(b => b.BreedName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("breed_name");
            
        builder.HasOne(b => b.Specie)
            .WithMany(s => s.Breeds)
            .HasForeignKey("specie_id")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
