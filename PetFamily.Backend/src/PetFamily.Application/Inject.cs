using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Species.Create;
using PetFamily.Application.Volunteers.AddHelpRequisites;
using PetFamily.Application.Volunteers.AddSocialNetworks;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.HardDelete;
using PetFamily.Application.Volunteers.Pets.AddPet;
using PetFamily.Application.Volunteers.Pets.GetPet;
using PetFamily.Application.Volunteers.Pets.RemovePet;
using PetFamily.Application.Volunteers.Pets.UploadFilesToPet;
using PetFamily.Application.Volunteers.SoftDelete;
using PetFamily.Application.Volunteers.UpdateMainInfo;

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
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}