using eCommerce.Core.Entities;

namespace eCommerce.Core.RepositoryContracts;

public interface IUsersRepository
{
    Task<Users?> AddUserAsync(Users user);
    Task<Users?> GetUserByEmailAsync(string email);
    Task<Users?> GetUserByIDAsync(Guid UserID);
    Task<Users?> UpdateUserAsync(Users user);
}
