namespace eCommerce.Core.DTOs;

public class AuthenticationResponse
{
  public Guid UserID;
  public string? Email;
  public string? PersonName;
  public string? Gender;
  public string? RefreshToken;//*
  public string? AccsesToken;//*
  public DateTimeOffset RefreshTokenExpiryTime;//*
  public bool Success;//*
  public AuthenticationResponse() : this(default, default, default,default, default, default,default, default)
  {
  }

    public AuthenticationResponse(Guid userID, string? email, string? personName, string? gender, string? refreshToken, string? accsesToken, DateTimeOffset refreshTokenExpiryTime, bool success)
    {
        UserID = userID;
        Email = email;
        PersonName = personName;
        Gender = gender;
        RefreshToken = refreshToken;
        AccsesToken = accsesToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
        Success = success;
    }
}
