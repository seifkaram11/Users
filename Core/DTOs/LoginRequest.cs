namespace eCommerce.Core.DTOs;

public record LoginRequest(
  string? Email,
  string? Password);
