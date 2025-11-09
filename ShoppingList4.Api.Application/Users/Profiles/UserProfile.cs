using AutoMapper;
using ShoppingList4.Api.Application.Users.Commands.RegisterUser;
using ShoppingList4.Api.Domain.Entities;

namespace ShoppingList4.Api.Application.Users.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserCommand, User>();
        }
    }
}