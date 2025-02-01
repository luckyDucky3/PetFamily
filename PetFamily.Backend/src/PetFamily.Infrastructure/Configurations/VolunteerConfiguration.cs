using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteer");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasConversion(id => id.Value, value => VolunteerId.Create(value))
            .IsRequired()
            .HasColumnName("volunteer_id");
        
        builder.ComplexProperty(v => v.Name, nb =>
        {
            nb.Property(n => n.FirstName)
                .IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("first_name");
            nb.Property(n => n.LastName)
                .IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("last_name");
            nb.Property(n => n.Patronymic)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
                .HasColumnName("patronymic");
        });
        
        builder.Property(v => v.Email).HasConversion(
            e => e!.Value, 
            v=> EmailAddress.CreateWithoutCheck(v))
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("email_address");
        
        builder.Property(v => v.Description)
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH)
            .HasColumnName("description");
        
        builder.Property(v => v.ExperienceYears)
            .HasColumnName("experience_years");
        
        builder.Ignore(v => v.CountOfPetsThatFindHome);
        
        builder.Ignore(v => v.CountOfPetsThatSearchHome);
        
        builder.Ignore(v => v.CountOfPetsThatSick);
        
        builder.Property(v => v.PhoneNumber)
            .HasConversion(
                p=>p!.Value,
                v=>new PhoneNumber(v))
            .IsRequired()
            .HasColumnName("phone_number");

        builder.Property(v => v.RequisitesForHelp).JsonValueObjectCollectionConversion();

        builder.Property(v => v.SocialNetworks).JsonValueObjectCollectionConversion();
    }
}