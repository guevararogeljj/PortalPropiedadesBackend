using LeadsInjection.Contracts;
using LeadsInjection.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeadsInjection
{
    public static class ServiceManager
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILeadService, LeadService>();

            return services;

        }
    }
}
