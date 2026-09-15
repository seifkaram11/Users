using Core.DTOs;
using Core.Entities;
using Core.Enum;

namespace Core.ServiceContracts;

public interface IUsersService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<IEnumerable<UserResponse>> GetUsersByConditionAsync(
    Func<Users,bool> func,
    int? pageNumber, int? pageSize,
    SortedType? sortedType, SortBy? sortBy);
    Task<bool> UpdateUserAsync(Guid userId, UpdateUserRequest user);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<UserResponse?> CreateUserAsync(AddUserRequest AddUserRequest);
    Task<bool> AddRoleAsync(AddRoleToUserRequest role);
}
