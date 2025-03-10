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
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<ILogger<UploadFilesHandler>> _loggerMock = new();
    private readonly Mock<IVolunteersRepository> _volunteerRepositoryMock = new();

    private readonly Mock<IValidator<UploadFilesToPetCommand>> _validatorMock = new();
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
        var specieId = SpecieId.Create(Guid.Parse("b1210781-568c-474d-83e5-67e2079b1e01"));
        var breedId = BreedId.Create(Guid.Parse("1e62d1cf-c0ad-429e-8d0a-f7fb4945b180"));
        var specieBreed = SpeciesBreeds.Create(specieId, breedId).Value;
        var address = Address.Create("state", "city", "street", "number").Value;
        var pet = Pet.Create(petId, name, specieBreed, Color.Black, address, description);
        volunteer.Value.AddPet(pet.Value);
        
        var stream = new MemoryStream();
        var fileName = "test.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileName);
        List<UploadFileDto> files = [uploadFileDto, uploadFileDto, uploadFileDto];
        var command = new UploadFilesToPetCommand(volunteerId, petId, files);

        IReadOnlyList<FilePath> filePaths =
        [
            FilePath.Create(Guid.NewGuid(), Path.GetExtension(fileName)).Value,
            FilePath.Create(Guid.NewGuid(), Path.GetExtension(fileName)).Value,
            FilePath.Create(Guid.NewGuid(), Path.GetExtension(fileName)).Value
        ];

        _fileProviderMock
            .Setup(v => v.UploadFiles(It.IsAny<List<FileData>>(), cancellationToken))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));
        
        _volunteerRepositoryMock
            .Setup(v => v.GetById(It.IsAny<VolunteerId>(), cancellationToken))
            .ReturnsAsync(volunteer.Value);
        
        _unitOfWorkMock.Setup(u => u.SaveChanges(cancellationToken))
            .Returns(Task.CompletedTask);

        _validatorMock.Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        var handler = new UploadFilesHandler(
            _fileProviderMock.Object,
            _loggerMock.Object,
            _volunteerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object);
        //act
        var result = await handler.Handle(command, cancellationToken);

        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet.Value.Id.Value);
    }
}