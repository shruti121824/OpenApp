﻿using Satrabel.OpenApp.Authorization.Users;
using AutoMapper;

namespace Satrabel.OpenApp.Users.Dto
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            CreateMap<CreateUserDto, User>();
            CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>();
            CreateMap<UpdateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore())
                                            .ForMember(x => x.Password, opt => opt.Ignore());
        }
    }
}
