using Azure.Storage.Blobs;
using BusinessLogic.Contracts;
using BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class ServiceManager
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<ICitiesService, CitiesService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IFiltersService, FiltersService>();
            services.AddScoped<IProceduralStageService, ProceduralStageService>();
            services.AddSingleton(new BlobServiceClient(configuration.GetConnectionString("BlobStorage").ToString()));
            services.AddSingleton<IAzureBlobService, AzureBlobService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<ISignUpUserService, SignUpUserService>();
            services.AddScoped<ISignInUserService, SignInUserService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddSingleton<IJWTService, JWTService>();
            services.AddSingleton<ICrmService, CrmService>();
            services.AddSingleton<ITFAService, TFAService>();
            services.AddSingleton<ISmsService, SmsService>();
            services.AddSingleton<IMaritalstatusService, MaritalstatusService>();
            services.AddSingleton<IOccupationService, OccupationService>();
            services.AddSingleton<IWebdoxRequestService, WebdoxRequestService>();
            services.AddSingleton<IIsiService, IsiService>();

            return services;

        }
    }
}
