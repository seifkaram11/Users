using eCommerce.Core.DTOs;
using eCommerce.Core.Entities;

namespace eCommerce.Core.ServiceContracts;

public interface IUsersService
{
    Task<TokenResponse?> LoginAsync(LoginRequest loginRequest);
    Task<AuthenticationResponse?> RegisterAsync(RegisterRequest registerRequest);
    Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequestDto request);
}
