using System.ComponentModel.DataAnnotations;
using System.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.Pets.PetDtos;
using PetFamily.Application.Volunteers.Pets.UploadFilesToPet;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models.Entities.Specie;
using PetFamily.Domain.Models.Entities.Volunteer;
using PetFamily.Domain.Models.Ids;
using PetFamily.Domain.Models.VO;
using PetFamily.Domain.Shared;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace PetFamily.Application.UnitTests;

public class UnitTest1
{
    [Fact]
    public async void Handle_Should_Upload_Files_To_Pet()
    {
        //arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("FirstName", "LastName").Value;
        var description = "Description";
        var emailAddress = EmailAddress.Create("test@test.com").Value;
        var phoneNumber = PhoneNumber.Create("89993331122").Value;
        var experienceYears = 0;

        var volunteer =
            Volunteer.Create(volunteerId, fullName, description, emailAddress, phoneNumber, experienceYears);

        var petId = PetId.NewPetId();
        var name = "test";
        var specieId = SpecieId.Create(Guid.NewGuid());
        var specieName = "testSpecie";
        var breedId = BreedId.Create(Guid.NewGuid());
        var breedName = "testBreed";
        var specie = Specie.Create(specieId, specieName);
        var breed = Breed.Create(breedId, breedName);
        var specieBreed = SpeciesBreeds.Create(specieId, breedId).Value;
        var address = Address.Create("state", "city", "strret", "number").Value;
        var pet = Pet.Create(petId, name, specieBreed, Color.Black, address, description);
        var stream = new MemoryStream();
        var fileName = "test.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileName);
        List<UploadFileDto> files = [uploadFileDto, uploadFileDto, uploadFileDto];
        var command = new UploadFilesToPetCommand(volunteerId, petId, files);
        var fileProviderMock = new Mock<IFileProvider>();

        IReadOnlyList<FilePath> filePaths =
        [
            FilePath.Create(Guid.NewGuid(), Path.GetExtension(fileName)).Value,
            FilePath.Create(Guid.NewGuid(), Path.GetExtension(fileName)).Value,
            FilePath.Create(Guid.NewGuid(), Path.GetExtension(fileName)).Value
        ];

        fileProviderMock
            .Setup(v => v.UploadFiles(It.IsAny<List<FileDataUpload>>(), cancellationToken))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));

        var loggerMock = new Mock<ILogger<UploadFilesHandler>>();
        //loggerMock.Setup(l => l.LogInformation(""));

        var volunteerRepositoryMock = new Mock<IVolunteersRepository>();
        volunteerRepositoryMock
            .Setup(v => v.GetById(It.IsAny<VolunteerId>(), cancellationToken))
            .ReturnsAsync(volunteer.Value);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.SaveChanges(cancellationToken))
            .Returns(Task.CompletedTask);

        var validatorMock = new Mock<IValidator<UploadFilesToPetCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        var handler = new UploadFilesHandler(
            fileProviderMock.Object,
            loggerMock.Object,
            volunteerRepositoryMock.Object,
            unitOfWorkMock.Object,
            validatorMock.Object);
        //act
        var result = await handler.Handle(command, cancellationToken);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Value.Id.Value);
    }
}