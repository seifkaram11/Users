namespace Core.DTOs;

public class UpdateUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
}
