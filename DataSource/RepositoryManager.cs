using DataSource.Contracts;
using DataSource.Interfaces;
using DataSource.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataSource
{
    public static class RepositoryManager
    {
        public static void AddRegistration(this IServiceCollection services)
        {
            services.AddScoped<IPropertiesRespository, PropertiesRespository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<IDocumentsRepository, DocumentsRepository>();
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IUserPropertiesRepository, UserPropertiesRepository>();
            services.AddScoped<IEmailValidationRepository, EmailValidationRepository>();
            services.AddScoped<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
            services.AddScoped<IOccupationRepository, OccupationRepository>();
            services.AddScoped<IMaritalstatusRepository, MaritalstatusRepository>();
            services.AddScoped<IWebdoxRequestRepository, WebdoxRequestRepository>();
            services.AddScoped<IParametersRepository, ParametersRepository>();
        }
    }
}
