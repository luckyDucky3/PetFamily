using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Models;
using PetFamily.Domain.Models.Entities;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteer");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasConversion(id => id.Value, value => VolunteerId.Create(value)).IsRequired().HasColumnName("volunteer_id");
        
        builder.Property(v => v.Name).HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH).HasColumnName("name");
        builder.Property(v => v.Email).HasConversion(e=>e.Value, v=> new EmailAdress(v)).HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH).HasColumnName("email");
        builder.Property(v => v.Description).IsRequired(false).HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH).HasColumnName("description");
        builder.Property(v => v.ExperienceYears).HasColumnName("experience_years");
        builder.Ignore(v => v.CountOfPetsThatFindHome);
        builder.Ignore(v => v.CountOfPetsThatSearchHome);
        builder.Ignore(v => v.CountOfPetsThatSick);
        builder.Property(v => v.PhoneNumber).HasConversion(p=>p.Value, v=>new PhoneNumber(v)).IsRequired().HasColumnName("phone_number");
        
        builder.OwnsOne(v => v.RequisitesForHelpDetails, vb =>
        {
            vb.ToJson();

            vb.OwnsMany(d => d.ListRequisitesForHelp, rb =>
            {
                rb.Property(r => r.value).IsRequired().HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
            });
        });
        builder.OwnsOne(v => v.SocialNetworksDetails, vb =>
        {
            vb.ToJson();
            vb.OwnsMany(s => s.SocialNetworks, sb =>
            {
                sb.Property(s=> s.Name).IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
                sb.Property(s => s.Link).IsRequired().HasMaxLength(Constants.MAX_SHORT_TEXT_LENGTH);
            });
        });
    }
}