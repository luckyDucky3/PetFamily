using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteer");
        builder.HasKey(v => v.Id);
        
        builder.HasMany(v => v.Pets)
            .WithOne(p => p.Volunteer)
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value, 
                value => VolunteerId.Create(value))
            .IsRequired();
        
        builder.ComplexProperty(v => v.Name, nb =>
        {
            nb.Property(n => n.FirstName)
                .IsRequired().HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH);
            nb.Property(n => n.LastName)
                .IsRequired().HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH);
            nb.Property(n => n.Patronymic)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH);
        });
        
        builder.Property(v => v.Email).HasConversion(
            e => e!.Value, 
            v=> EmailAddress.CreateWithoutCheck(v))
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("email_address");
        
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("description");
        
        builder.Property(v => v.ExperienceYears)
            .HasColumnName("experience_years");
        
        builder.Ignore(v => v.CountOfPetsThatFindHome);
        
        builder.Ignore(v => v.CountOfPetsThatSearchHome);
        
        builder.Ignore(v => v.CountOfPetsThatSick);
        
        builder.Property(v => v.PhoneNumber)
            .HasConversion(
                p => p!.Value,
                v => PhoneNumber.CreateWithoutCheck(v))
            .IsRequired()
            .HasColumnName("phone_number");

        builder.Property(v => v.HelpRequisites)
            .JsonValueObjectCollectionConversion()
            .HasColumnName("help_requisites");

        builder.Property(v => v.SocialNetworks)
            .JsonValueObjectCollectionConversion();
        
        builder.Property<bool>("IsDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.Property<DateTime>("DeletionDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .SetDateTimeKind(DateTimeKind.Utc)
            .HasColumnName("deletion_date");
    }
}