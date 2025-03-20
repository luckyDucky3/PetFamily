using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Dtos;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteer");
        builder.HasKey(v => v.Id);
        builder.HasMany<PetDto>()
            .WithOne()
            .HasForeignKey(p => p.VolunteerId)
            .IsRequired();
        builder.ComplexProperty(v => v.Name);
    }
}