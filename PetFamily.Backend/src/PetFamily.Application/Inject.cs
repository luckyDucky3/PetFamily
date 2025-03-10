using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Species.Create;
using PetFamily.Application.Volunteers.Commands.AddHelpRequisites;
using PetFamily.Application.Volunteers.Commands.AddSocialNetworks;
using PetFamily.Application.Volunteers.Commands.Create;
using PetFamily.Application.Volunteers.Commands.HardDelete;
using PetFamily.Application.Volunteers.Commands.SoftDelete;
using PetFamily.Application.Volunteers.Commands.UpdateMainInfo;
using PetFamily.Application.Volunteers.Pets.Commands.AddPet;
using PetFamily.Application.Volunteers.Pets.Commands.RemovePet;
using PetFamily.Application.Volunteers.Pets.Commands.UploadFilesToPet;
using PetFamily.Application.Volunteers.Pets.Queries.GetPet;
using PetFamily.Application.Volunteers.Pets.Queries.GetPetsWithPagination;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<AddHelpRequisitesHandler>();
        services.AddScoped<AddSocialNetworksHandler>();
        services.AddScoped<HardDeleteVolunteerHandler>();
        services.AddScoped<SoftDeleteVolunteerHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<RemovePetHandler>();
        services.AddScoped<GetPetHandler>();
        services.AddScoped<CreateSpecieHandler>();
        services.AddScoped<UploadFilesHandler>();
        services.AddScoped<GetPetsWithPaginationHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}