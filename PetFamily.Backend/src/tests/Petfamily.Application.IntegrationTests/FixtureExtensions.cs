using AutoFixture;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.Pets.Commands.UploadFilesToPet;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;

namespace Petfamily.Application.IntegrationTests;

public static class FixtureExtensions
{
    public static Volunteer CreateVolunteer(this Fixture fixture, Guid volunteerId)
    {
        return fixture.Build<Volunteer>().With(v => v.Id, VolunteerId.Create(volunteerId)).Create();
    }

    public static Pet CreatePet(this Fixture fixture, Guid petId)
    {
        return fixture.Build<Pet>().With(p => p.Id, PetId.Create(petId)).Create();
    }
    public static UploadFilesToPetCommand CreateUploadFilesToPetCommand(
        this Fixture fixture,
        Guid volunteerId,
        Guid petId,
        string fileName)
    {
        UploadFileDto u = fixture.Build<UploadFileDto>().With(d => d.FileName, fileName).Create();
        IEnumerable<UploadFileDto> collection = [u, u, u];
        return fixture.Build<UploadFilesToPetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.PetId)
            .With(c => c.PetId, petId)
            .With(c => c.Files, collection)
            .Create();
    }
}