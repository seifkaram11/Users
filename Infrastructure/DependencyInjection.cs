using Core.RepositoryContracts;
using Infrastructure.DbContext;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository,UsersRepository>();
        services.AddScoped<DapperDbContext>();
        services.AddScoped<IRolesRepository,RolesRepository>();
        return services;
    }
}
