using Core.Entities;

namespace Core.ServiceContracts;

public interface IRolesService
{
    Task<IEnumerable<Roles>> GetAllRolesAsync();
    public Task<bool> UpdateUserRolesAsync(Guid userId, IEnumerable<string> roleNames);
    Task<bool>UpdateRole(Guid roleId,string newRoleName);
    Task<IEnumerable<Roles>> GetRolesByConditionAsync(Func<Roles,bool> func);
    Task<bool> AddRoleAsync(string role);
    Task<IEnumerable<Roles>> GetUserRoles(Guid userId);
    Task<IEnumerable<Guid>> GetRolesIdsByNameAsync(IEnumerable<string> roles);
}
