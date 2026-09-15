using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enum;
using Core.RepositoryContracts;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace Core.Services;

public class UsersService : IUsersService
{
    IUsersRepository _usersRepository;
    IRolesService _rolesService;
    readonly IPasswordHasher<Users> _passwordHasher;
    IMapper _mapper;

    public UsersService(IUsersRepository usersRepository, IPasswordHasher<Users> passwordHasher, IMapper mapper, IRolesService rolesService)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _rolesService = rolesService;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        return await _usersRepository.DeleteUserAsync(userId);
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        List<UserResponse> userResponses=new ();
        var users=await _usersRepository.GetAllUsersAsync();
        foreach(var user in users)
        {
            var roles=await _rolesService.GetUserRoles(user.UserID);
            userResponses.Add(new UserResponse(){Email=user.Email!, Gender=user.Gender!,Name=user.Name!,Roles=roles});
        }
        return userResponses;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersByConditionAsync(Func<Users, bool> func, int? pageNumber, int? pageSize, SortedType? sortedType, SortBy? sortBy)
    {
        var users = await _usersRepository.GetAllUsersAsync();
        var filteredUsers = users.Where(func);

        if (sortedType is null || sortBy is null)
        {
            return filteredUsers.Select(_=>_mapper.Map<UserResponse>(_));
        }

        filteredUsers = sortedType switch
        {
            SortedType.Ascending => sortBy switch
            {
                SortBy.Name => filteredUsers.OrderBy(u => u.Name),
                SortBy.Email => filteredUsers.OrderBy(u => u.Email),
                SortBy.Gender => filteredUsers.OrderBy(u => u.Gender),
                SortBy.RefreshTokenExpiryTime => filteredUsers.OrderBy(u => u.RefreshTokenExpiryTime),
                _ => filteredUsers
            },
            SortedType.Descending => sortBy switch
            {
                SortBy.Name => filteredUsers.OrderByDescending(u => u.Name),
                SortBy.Email => filteredUsers.OrderByDescending(u => u.Email),
                SortBy.Gender => filteredUsers.OrderByDescending(u => u.Gender),
                SortBy.RefreshTokenExpiryTime => filteredUsers.OrderByDescending(u => u.RefreshTokenExpiryTime),
                _ => filteredUsers
            },
            _ => filteredUsers
        };

        if(pageNumber is null || pageSize is null || pageNumber <= 0 || pageSize <= 0)
        {
            return filteredUsers.Select(_=>_mapper.Map<UserResponse>(_));
        }
        int pageNumberValue = pageNumber.Value;
        int pageSizeValue = pageSize.Value;
        var paginatedUsers = filteredUsers.Skip((pageNumberValue - 1) * pageSizeValue).Take(pageSizeValue);

        return paginatedUsers.Select(_=>_mapper.Map<UserResponse>(_));
    }

    public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserRequest updateUserRequest)
    {

        if (updateUserRequest is null) return false;
        // Users users = new Users
        // {
        //     UserID = userId,
        //     Email = updateUserRequest.Email,
        //     Name = updateUserRequest.Name,
        // };

        Users? users=await _usersRepository.GetUserByIDAsync(userId);
        if(users is null)return false;
        users.Email=updateUserRequest.Email;
        users.Name=updateUserRequest.Name;
        users.UserID = userId;
        users.PasswordHash=_passwordHasher.HashPassword(users,updateUserRequest.Password!);
        var res = await _usersRepository.UpdateUserAsync(users);
        return res is not null;
    }

    public async Task<UserResponse?> CreateUserAsync(AddUserRequest AddUserRequest)
    {
        var roleIds = await _rolesService.GetRolesIdsByNameAsync(AddUserRequest.RoleNames);
        if(AddUserRequest is null) return null;
        Users user=new Users
        {
            UserID=Guid.NewGuid(),
            Email = AddUserRequest.Email,
            Name = AddUserRequest.Name,
            Gender = AddUserRequest.Gender.ToString(),
        };
        user.PasswordHash=_passwordHasher.HashPassword(user,AddUserRequest.Password!);
        var res=await _usersRepository.AddUserAsync(user);
        if(res is null) return null;
        foreach(var roleId in roleIds)
        {
            await _usersRepository.AddRoleToUserAsync(user.UserID,roleId);
        }
        var rolesForUser=await _rolesService.GetUserRoles(user.UserID);
        if(rolesForUser is null) return _mapper.Map<UserResponse>(res);
        res.Roles=rolesForUser;
        return _mapper.Map<UserResponse>(res);
    }

    public async Task<bool> AddRoleAsync(AddRoleToUserRequest role)
    {
        if(role.RoleId == Guid.Empty) return false;
        return await _usersRepository.AddRoleToUserAsync(role.UserId, role.RoleId);
    }
}
