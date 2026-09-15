using Core.Entities;
using Core.RepositoryContracts;
using Core.ServiceContracts;

namespace Core.Services;

public class RolesService : IRolesService
{
    IRolesRepository _rolesRepository;

    public RolesService(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task<IEnumerable<Roles>> GetAllRolesAsync()
    {
        var roles = await _rolesRepository.GetAllRolesAsync();
        return roles;
    }

    public async Task<bool> UpdateRole(Guid roleId, string newRoleName)
    {
        return await _rolesRepository.UpdateRoleAsync(roleId, newRoleName);
    }

    public async Task<bool> UpdateUserRolesAsync(Guid userId, IEnumerable<string> roleNames)
    {
        return await _rolesRepository.UpdateUsersRolesAsync(userId, roleNames);
    }

    public async Task<IEnumerable<Roles>> GetRolesByConditionAsync(Func<Roles,bool> func)
    {
        var roles = await _rolesRepository.GetAllRolesAsync();
        return roles.Where(func);
    }

    public async Task<bool> AddRoleAsync(string roleName)
    {
        var role = new Roles
        {
            RoleId = Guid.NewGuid(),
            RoleName = roleName
        };
        return await _rolesRepository.AddRoleAsync(role);
    }

    public async Task<IEnumerable<Roles>> GetUserRoles(Guid userId)
    {
        return await _rolesRepository.GetRolesByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Guid>> GetRolesIdByNames(IEnumerable<string> roles)
    {
        return await _rolesRepository.GetRolesIdsByNameAsync(roles);
    }

    public async Task<IEnumerable<Guid>> GetRolesIdsByNameAsync(IEnumerable<string> roles)
    {
        return await _rolesRepository.GetRolesIdsByNameAsync(roles);
    }
}
