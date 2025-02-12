using FluentValidation;
using PetFamily.Application.Volunteers.Dto;
using PetFamily.Application.Volunteers.Validation;

namespace PetFamily.Application.Volunteers.AddHelpRequisites;

public record AddHelpRequisitesRequest(Guid Id, ListHelpRequisiteDto HelpRequisiteDtos);