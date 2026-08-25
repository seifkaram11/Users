using System.Security.Cryptography;
using eCommerce.Core.DTOs;
using eCommerce.Core.Entities;
using eCommerce.Core.ServiceContracts;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using eCommerce.Core.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace eCommerce.Core.Services;

class AuthService : IAuthService
{
    IConfiguration _configuration;
    IUsersRepository _usersRepository;
    readonly IPasswordHasher<Users> _passwordHasher;
    IMapper _mapper;

    public AuthService(IConfiguration configuration, IUsersRepository usersRepository, IPasswordHasher<Users> passwordHasher, IMapper mapper)
    {
        _configuration = configuration;
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<TokenResponse?> LoginAsync(LoginRequest loginRequest)
    {
        if(loginRequest is null)return null;

        var user =await _usersRepository.GetUserByEmailAsync(loginRequest.Email ?? "");

        if(user is null ||
        _passwordHasher.VerifyHashedPassword(user,user.PasswordHash!,loginRequest.Password!) == PasswordVerificationResult.Failed)
            return null;

        return await GenreateTokenResponseAsync(user);

    }

    async Task<TokenResponse> GenreateTokenResponseAsync(Users user)
    {
        return new TokenResponse
        {
            RefreshToken= await GenerateAndSaveRefreshTokenAsync(user),
            AccessToken= await createToken(user)
        };
    }

    async Task<Users?> ValidateRefreshToken(string RefreshToken,Guid UserId)
    {
        var user=await _usersRepository.GetUserByIDAsync(UserId);

        if(user is null || user.RefreshToken!=RefreshToken || user.RefreshTokenExpiryTime < DateTimeOffset.UtcNow)return null;

        return user;
    }
    async Task<string> GenerateAndSaveRefreshTokenAsync(Users user)
    {
        string theRefreshToken=GenerateRefreshToken();
        user.RefreshToken=theRefreshToken;
        user.RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddDays(3);
        await _usersRepository.UpdateUserAsync(user);
        return theRefreshToken;
    }
    string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
    public async Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        if(request is null ||  string.IsNullOrEmpty(request.RefreshToken))return null;
        Users? user=await ValidateRefreshToken(request.RefreshToken,request.UserId);
        if(user is null)return null;

        return await GenreateTokenResponseAsync(user);
    }

    public async Task<AuthenticationResponse?> RegisterAsync(RegisterRequest register)
    {
        string refreshToken=GenerateRefreshToken();
        Users user=new()
        {
            UserID=Guid.NewGuid(),
            Email= register.Email,
            Name=register.PersonName,
            RefreshTokenExpiryTime=DateTimeOffset.UtcNow.AddDays(3),
            RefreshToken=refreshToken,
            Gender=register.Gender.ToString()
        };
        var res=_mapper.Map<AuthenticationResponse>(user);

        user.PasswordHash=_passwordHasher.HashPassword(user,register.Password!);
        var x=await _usersRepository.AddUserAsync(user);
        if(x is null)return null;
        res.Success=true;
        return res;
    }

    async Task<string> createToken(Users user)
    {
        var claims=new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name??""),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Gender, user.Gender??""),
            new Claim(ClaimTypes.Email, user.Email?? "empty@empty.com")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Auth:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Auth:Issuer"),
                audience: _configuration.GetValue<string>("Auth:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

        var res= new JwtSecurityTokenHandler().WriteToken(token);

        return res;
    }
}
