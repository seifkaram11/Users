using eCommerce.Core.Entities;
using eCommerce.Core.ServiceContracts;
using eCommerce.Core.Services;
using eCommerce.Core.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUsersService,UsersService>();
        services.AddScoped<IAuthService,AuthService>();
        services.AddScoped<IPasswordHasher<Users>,PasswordHasher<Users>>();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        return services;
    }
}
