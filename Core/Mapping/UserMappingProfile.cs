using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Core.Mapping;

public class UserMappingProfile:Profile
{
    public UserMappingProfile()
    {
        CreateMap<Users,UserResponse>()
        .ForMember(dest=>dest.Roles,op=>op.Ignore());
    }
}
