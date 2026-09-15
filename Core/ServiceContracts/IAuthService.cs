using Core.DTOs;

namespace Core.ServiceContracts;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginRequest loginRequest);
    Task<AuthenticationResponse?> RegisterAsync(RegisterRequest users);
    Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequestDto request);
}

