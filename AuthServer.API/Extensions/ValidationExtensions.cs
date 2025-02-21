using AuthServer.API.Validations;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace AuthServer.API.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

            return services;
        }
    }
}
