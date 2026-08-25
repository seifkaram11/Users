namespace eCommerce.Core.Entities;

public class Users
{
  public Guid UserID { get; set; }
  public string? Email { get; set; }
  public string? PasswordHash { get; set; }
  public string? Name { get; set; }
  public string? Gender { get; set; }
  public string? RefreshToken{get;set;}
  public DateTimeOffset RefreshTokenExpiryTime{get;set;}
}
