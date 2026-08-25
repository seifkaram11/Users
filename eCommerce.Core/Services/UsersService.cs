using eCommerce.Core.DTOs;
using eCommerce.Core.ServiceContracts;

namespace eCommerce.Core.Services;

public class UsersService : IUsersService
{
    IAuthService _authService;

    public UsersService(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenResponse?> LoginAsync(LoginRequest loginRequest)
    {
        return await _authService.LoginAsync(loginRequest);

    }

    public async Task<AuthenticationResponse?> RegisterAsync(RegisterRequest registerRequest)
    {
        return await _authService.RegisterAsync(registerRequest);
    }

    public async Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        return await _authService.RefreshTokensAsync(request);
    }
}
