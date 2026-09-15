using Core.Entities;

namespace Core.DTOs;

public class UserResponse
{
    public string Email{get;set;}=null!;
    public string Name{get;set;}=null!;
    public string Gender{get;set;}=null!;
    public IEnumerable<Roles> Roles{get;set;}=new List<Roles>();
}
