using PetFamily.Application.Volunteers.Dto;

namespace PetFamily.Application.Volunteers.AddHelpRequisites;

public record AddHelpRequisitesCommand(Guid Id, List<HelpRequisiteDto> HelpRequisiteDtos);