using Core.Enum;

namespace Core.DTOs;

public class AddUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Genders Gender { get; set; }
    public IEnumerable<string> RoleNames { get; set; } = new List<string>();
}
